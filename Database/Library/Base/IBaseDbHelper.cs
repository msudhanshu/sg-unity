using System;
using System.Collections.Generic;

namespace KiwiCommonDatabase
{
	public interface IBaseDbHelper	
	{
		int ExecuteSql(string sql, params object[] args);

		void CreateTableIfNotExists<T>();

		void DropTable<T>();

		void UpdateTableSchema<T>();

		//Basic CRUD methods

		List<T> Query<T>(string query, params object[] args) where T: new();	//constraint that type T should have parameterless constructor

		int Insert<T>(object obj);

		int Delete<T>(T obj);

		int Update<T>(object obj);
		 
		int InsertOrUpdate<T>(object obj);

		//Additional Helper methods

		T QueryObjectById<T>(object idValue) where T: new();

		List<T> QueryForAll<T>() where T: new();

		void DeleteObjectById<T>(object idValue) where T: new();

		//DB Helper methods via KDbQuery
		List<T> QueryForAll<T>(KDbQuery<T> dbquery) where T:BaseDbModel, new();

		T QueryForFirst<T>(KDbQuery<T> dbquery) where T:BaseDbModel, new();

		void DeleteAll<T>(KDbQuery<T> dbquery) where T:BaseDbModel, new();

		//Invoke DbHelper Generic Method by Reflection
		object InvokeGenericMethodByReflection(string className, string methodName, object[] args);

	}
}

