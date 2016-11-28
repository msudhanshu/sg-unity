using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using SimpleSQL;
using System.Reflection;
using System.Runtime;
using System.Linq;

namespace KiwiCommonDatabase
{
		public class SimpleSqlDbHelper: IBaseDbHelper
		{
				protected static SimpleSqlDbHelper dbHelper = null;
		
				protected static SimpleSQLManager dbManager;

				private enum EQueryMode{
					QUERY,
					DELETE
				};

				public static SimpleSqlDbHelper GetInstance ()
				{
						if (dbHelper == null) {
								dbHelper = new SimpleSqlDbHelper ();
						}
						return dbHelper;
				}

				protected SimpleSqlDbHelper ()
				{
				}

				public void SetSimpleSqlManager (SimpleSQLManager ssManager)
				{
						dbManager = ssManager;
				}

				int IBaseDbHelper.ExecuteSql (string sql, params object[] args)
				{
						try {
								return dbManager.Execute (sql, args);
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
						return 0;
				}
		
				void IBaseDbHelper.CreateTableIfNotExists<T> ()
				{
						try {
								dbManager.CreateTable<T> ();
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
				}

				void IBaseDbHelper.DropTable<T> ()
				{
						try {
								string sql = "drop table " + typeof(T).ToString ();
								((IBaseDbHelper)this).ExecuteSql (sql);	
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
				}

				void IBaseDbHelper.UpdateTableSchema<T> ()
				{
						((IBaseDbHelper)this).DropTable<T> ();
						((IBaseDbHelper)this).CreateTableIfNotExists<T> ();
				}

				List<T> IBaseDbHelper.Query<T> (string query, params object[] args)
				{
						try {
								return dbManager.Query<T> (query, args);	
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
						return null;
				}

				int IBaseDbHelper.Insert<T> (object obj)
				{
						try {
								return dbManager.Insert (obj);	
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
						return 0;
				}
		
				int IBaseDbHelper.Delete<T> (T obj)
				{
						try {
								return dbManager.Delete<T> (obj);				
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
						return 0;
				}

				/**
		 * Updates all of the columns of a table using the specified object except for its primary key. 
		 * The object is required to have a primary key.
		 **/
				int IBaseDbHelper.Update<T> (object obj)
				{
						try {
								return dbManager.UpdateTable (obj);	
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
						return 0;
				}

			int IBaseDbHelper.InsertOrUpdate<T> (object obj)
			{
				try {
					return dbManager.Insert (obj);    
				} catch (SQLiteException sqx) {
				Debug.LogWarning ("This "+obj.ToString()+"'s entry already present");
					try {
						return dbManager.UpdateTable (obj);    
					} catch (Exception ex) {
						Debug.LogException (ex);
					}
				} catch (Exception ex1) {
					Debug.LogException (ex1);
				}
				
				return 0;
			}


				//Additional Helper methods

				T IBaseDbHelper.QueryObjectById<T> (object idValue)
				{
						try {
								string sql = "select * from " + typeof(T) + " where id = ?";
								List<T> results = ((IBaseDbHelper)this).Query<T> (sql, idValue);
								if (results != null && results.Count > 0) {
										return results [0];
								}
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
						return default(T);
				}

				List<T> IBaseDbHelper.QueryForAll<T> ()
				{
						try {
								string sql = "select * from " + typeof(T).ToString ();
								return ((IBaseDbHelper)this).Query<T> (sql); 	
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
						return null;
				}

				void IBaseDbHelper.DeleteObjectById<T> (object idValue)
				{
						try {
								string sql = "delete * from " + typeof(T).ToString () + " where id = ?";
								((IBaseDbHelper)this).ExecuteSql (sql, idValue);	
						} catch (Exception ex) {
								Debug.LogException (ex);
						}
				}

		//DB Helper private methods via KDbQuery
		private string SqlForQuery<T>(KDbQuery<T> dbquery, EQueryMode mode) where T:BaseDbModel, new(){
			string querySql = mode == EQueryMode.QUERY ? "select " : "delete";
			KDbQueryAttribute dbqueryAttribute = dbquery.getQueryAttribute ();
			
			if(dbqueryAttribute != null && dbqueryAttribute.hasSelectedColumns()){
				foreach(string columnName in dbqueryAttribute.getSelectedColumnsArray()){
					querySql += (columnName + " ,");
				}
				querySql = querySql.Remove(querySql.Length - 1);	//Remove extra comma from end
			}else{
				if(mode == EQueryMode.QUERY){
					querySql += (" * ");
				}
			}

			querySql += (" from " + dbquery.getDbClass());

			//Update where clause
			querySql += this.WherePredicateForQueryOps (dbquery.getAndedDbOperationsArray());

			//Update order by, limit, offset attributes of db query
			if(mode == EQueryMode.QUERY && dbqueryAttribute != null){
				
				if(dbqueryAttribute.hasOrderByColumns()){
					querySql += " order by ";
					foreach(OrderByValue orderByValue in dbqueryAttribute.getOrderByColumnsList()){
						querySql += (orderByValue.getName()+ (orderByValue.getValue()? " ASC " : " DESC ") +" ,");
					}
					querySql = querySql.Remove(querySql.Length - 1);	//Remove extra comma from end
				}
				
				if(dbqueryAttribute.hasNonDefaultLimit()){
					querySql += (" limit " + dbqueryAttribute.getLimit());

					//offset is used only with limit
					if(dbqueryAttribute.hasNonDefaultOffset()){
						querySql += (" offset " + dbqueryAttribute.getOffset());
					}
				}
				

			}

			return querySql;
		}


		private string WherePredicateForQueryOps(BaseDbOp[] andedDbOps){
			string whereSql = "";
			if(andedDbOps != null && andedDbOps.Length > 0){
				whereSql = " where ";

				for(int i=0; i< andedDbOps.Length; i++){
					BaseDbOp op = andedDbOps[i];
					whereSql += this.WherePredicateForQueryOp(op);

					if(i < andedDbOps.Length -1){
						BaseDbOp nextOp = andedDbOps[i+1];
						if(!(nextOp is CompoundDbOp)){
							whereSql += " and ";
						}
					}
				}
			}
			return whereSql;
		}


		private string WherePredicateForQueryOp(BaseDbOp dbOp){
			if(dbOp is CompoundDbOp){
				return " (" + this.WherePredicateForCompoundDbOp((CompoundDbOp)dbOp) + ") ";
			}else{
				return " (" + this.WherePredicateForBasicDbOp(dbOp) + ") ";
			}
		}


		private string WherePredicateForBasicDbOp(BaseDbOp op){
			string whereSql = "";
			string OP_CODE = " " + DbOperatorExtension.getOpCode(op.getOp()) + " ";
			object arg1, arg2 = null;
			try {
				switch(op.getOp()){
				
				case EDbOperators.EQUALS:
				case EDbOperators.LESS_THAN:
				case EDbOperators.LESS_THAN_EQUAL_TO:
				case EDbOperators.GREATER_THAN:
				case EDbOperators.GREATER_THAN_EQUAL_TO:
				case EDbOperators.NOT_EQUAL:
				case EDbOperators.LIKE:
				case EDbOperators.NOT_LIKE:
					arg2 = ((BinaryDbOp)op).getArg2();
					whereSql = ((string) ((BinaryDbOp)op).getArg1() + OP_CODE + this.ObjectValueInSql(arg2));
					break;

				case EDbOperators.IS_NULL:
				case EDbOperators.IS_NOT_NULL:
					whereSql = (string) ((UnaryDbOp)op).getArg1() + OP_CODE;
					break;
					
				case EDbOperators.IN:
				case EDbOperators.NOT_IN:
					arg2 = ((BinaryDbOp)op).getArg2();
					if(arg2 is ArrayList){
						ArrayList arg2List = (ArrayList) ((BinaryDbOp)op).getArg2();
						if(arg2List != null && arg2List.Count > 0){
							whereSql = (string) ((BinaryDbOp)op).getArg1() + OP_CODE + "( ";
							foreach(object obj in arg2List){
								whereSql += (this.ObjectValueInSql(obj) + " ,");
							}
							whereSql = whereSql.Remove(whereSql.Length - 1);	//Remove extra comma from end
							whereSql += " ) ";
						}
					}
					break;
					
//				case DbOperators.AND_CONJOIN:
//					break;
//					
//				case DbOperators.OR_CONJOIN:
//					break;
					
				case EDbOperators.AND:
				case EDbOperators.OR:
//					throw new RuntimeException("Don't pass compound operations - AND/ OR in this basic operations method. Use updateWhereForCompoundOperations instead.");
					break;
				}
			} catch (Exception e) {
				// TODO Auto-generated catch block

			}
			return whereSql;
		}

		private string WherePredicateForCompoundDbOp(CompoundDbOp dbOp){
			BaseDbOp[] dbOps = dbOp.getCompoundOperands ();
			string OP_CODE = " " + DbOperatorExtension.getOpCode (dbOp.getOp ()) + " ";
			string whereSql = "";
			if(dbOps != null && dbOps.Length > 0){
				for(int i = 0; i < dbOps.Length; i++){
					whereSql += this.WherePredicateForQueryOp(dbOps[i]);
					if(i < dbOps.Length -1){
						whereSql += OP_CODE;
					}
				}
			}
			
			return whereSql;
		}

		private object ObjectValueInSql(object obj){
			if(obj is string){
				return ("'" + obj + "'");	//Enclose string value in single quotes
			}else{
				return obj;
			}
		}
		
		//DB Helper public methods via KDbQuery
		List<T> IBaseDbHelper.QueryForAll<T>(KDbQuery<T> dbquery){
			string querySql = this.SqlForQuery<T> (dbquery, EQueryMode.QUERY);
			Debug.Log ("Query SQL after parsing KDbQuery = " + querySql);
			return ((IBaseDbHelper)this).Query<T>(querySql);
		}


		T IBaseDbHelper.QueryForFirst<T>(KDbQuery<T> dbquery){
			KDbQueryAttribute queryAttribute = dbquery.getQueryAttribute ();
			if (queryAttribute == null)
				queryAttribute = new KDbQueryAttribute();
			queryAttribute.setLimit (1);	//Set limit to return only ONE item in result
			dbquery.setQueryAttribute (queryAttribute);
			List<T> resultItems = ((IBaseDbHelper)this).QueryForAll<T>(dbquery);
			if(resultItems != null && resultItems.Count > 0){
				return resultItems[0];
			}
			return default(T);
		}


		void IBaseDbHelper.DeleteAll<T>(KDbQuery<T> dbquery){
			string querySql = this.SqlForQuery<T> (dbquery, EQueryMode.DELETE);
			Debug.Log ("Query SQL for DELETE after parsing KDbQuery = " + querySql);
			((IBaseDbHelper)this).ExecuteSql (querySql);
		}

		//Invoke DbHelper Generic Method by Reflection
		object IBaseDbHelper.InvokeGenericMethodByReflection(string className, string methodName, object[] args){
			System.Type dtype = System.Type.GetType (className);
			System.Type dbInterface = dbHelper.GetType ().GetInterfaces ().Single (e => e.Name == "IBaseDbHelper");
			MethodInfo method = null;
			if (dtype != null && dbInterface != null) {
				method = dbInterface.GetMethod (methodName).MakeGenericMethod (new System.Type[] { dtype });
			}
			return method.Invoke(this, args);
		}


	}
}

