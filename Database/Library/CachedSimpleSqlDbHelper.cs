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
		public class CachedSimpleSqlDbHelper: SimpleSqlDbHelper, IBaseDbHelper
		{

		public static CachedSimpleSqlDbHelper GetInstance ()
		{
			if(dbHelper==null)
				dbHelper = new CachedSimpleSqlDbHelper();
			return (CachedSimpleSqlDbHelper)dbHelper;
		}

		private CachedSimpleSqlDbHelper() : base() {}

			private Dictionary<Type, Dictionary<object,object>> cache; 
				//Additional Helper methods

			T IBaseDbHelper.QueryObjectById<T> (object idValue)
			{
				
				if(cache==null)
					cache = new Dictionary<Type, Dictionary<object, object>>();
				if(cache.ContainsKey(typeof(T))){
					if(idValue!=null && cache[typeof(T)].ContainsKey(idValue))
						return (T)cache[typeof(T)][idValue];
				}
				else{
					cache[typeof(T)] = new Dictionary<object, object>();
				}
				try {
					string sql = "select * from " + typeof(T) + " where id = ?";
					List<T> results = ((IBaseDbHelper)this).Query<T> (sql, idValue);
					if (results != null && results.Count > 0) {
							cache[typeof(T)].Add (idValue,results[0]);
							return results [0];
					}
				} catch (Exception ex) {
						Debug.LogException (ex);
				}
				return default(T);
			}

			void IBaseDbHelper.DeleteObjectById<T> (object idValue)
			{
					
				try {
					string sql = "delete * from " + typeof(T).ToString () + " where id = ?";
					((IBaseDbHelper)this).ExecuteSql (sql, idValue);
				} catch (Exception ex) {
					Debug.LogException (ex);
				}
				if(cache!=null){
					if(cache.ContainsKey(typeof(T))){
					cache[typeof(T)].Remove(idValue);
					}
				}
			}

//			List<T> IBaseDbHelper.Query<T> (string query, params object[] args)
//			{
//				List<T> resultsList = ((IBaseDbHelper)base).Query<T>(query, args);
//
//
//				return resultsList;
//			}


			void clear(){
				if(cache!=null){
					cache.Clear();
					cache = null;
				}
			}
		}
	
	}

