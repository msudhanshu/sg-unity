using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using KiwiCommonDatabase;
using SgUnity;

public class SeedData
{
		public static void SeedPendingDownloads ()
		{
				for (int i = 0; i < 4; i++) {
						string bundleName = "model_" + i;
						PendingDownload.Add (bundleName, 5 - i, 0);
				}
				

		}

		public static void SeedDataFromResources() {
			throw new Exception ("SeedClass<CategoryState>(); called and crashed");
			//SeedClass<CategoryState>();			
		}

		private static void SeedClass<T>() {
			if (ServerConfig.SERVER_ENABLED)
				return;

			TextAsset textAsset = Resources.Load(typeof(T).ToString()) as TextAsset;
			List<T> objects = JsonConvert.DeserializeObject<List<T>>(textAsset.text);
			
			IBaseDbHelper dbHelper = DatabaseManager.GetInstance().GetDbHelper();
			dbHelper.DropTable<T>();
			dbHelper.CreateTableIfNotExists<T>();

			if (objects == null) 
				return;
			for (int i = 0; i < objects.Count; i++) {
				dbHelper.Insert<T>(objects[i]);
			}
			Resources.UnloadAsset(textAsset);
		}
}

