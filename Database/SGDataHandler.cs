using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Reflection;
using KiwiCommonDatabase;
using Expansion;
using SgUnity;
namespace KiwiCommonDatabase
{
    
	public class SGDataHandler <M,T> : Manager<M> 
		where M : Manager<M>
		where T : ISGDataWrapper {

	public bool USE_PACKED_DB = true;
	public TextAsset packedDb;

	public static T wrapper = null;
	bool diffRequestCompleted = false;
	bool diffRequestSucceeded = false;
    string diffUrl = ServerConfig.BASE_URL+"diff?user_id=<user_id>&version=<version>";
    string newUserUrl =  ServerConfig.BASE_URL+ "user/new?email=<email>&device_id=<device>&app_version=1";

	string USERID_TAG = "<user_id>";
	string VERSION_TAG = "<version>";
	string EMAIL_TAG = "<email>";
	string DEVICE_TAG = "<device>";
	//FIXME Temp solution for not showing UI till diff comes
	public GameObject Canvas;

	override public void StartInit() {
		if (ServerConfig.SERVER_ENABLED) {
			StartCoroutine(GetDiff());
		} else {
			SeedData.SeedDataFromResources();
			Finished();
		}
	}

	override public void PopulateDependencies() {
            dependencies = new List<ManagerDependency>();
            dependencies.Add(ManagerDependency.DATABASE_INITIALIZED);
	}


	IEnumerator GetDiff() {
		int userId = PlayerPrefs.GetInt(Config.USER_ID_KEY, -1);
		if (userId == -1) {
			newUserUrl = newUserUrl.Replace(EMAIL_TAG, SystemInfo.deviceName).Replace(DEVICE_TAG, SystemInfo.deviceUniqueIdentifier);
			Debug.LogWarning("User URL = " + newUserUrl);

			WWW newUserRequest = new WWW(newUserUrl);
			yield return newUserRequest;

			if (newUserRequest.error != null) {
				Debug.LogError("Failed in new user URL");
				yield return -1;
			}

			NewUserResponse response = JsonConvert.DeserializeObject<NewUserResponse>(newUserRequest.text);
			userId = Convert.ToInt32(response.itemId);
            Debug.LogWarning("User Id created = " + userId);
			PlayerPrefs.SetInt(Config.USER_ID_KEY, userId);
		}

		Config.USER_ID = userId;
		diffUrl = diffUrl.Replace(USERID_TAG, string.Format("" + userId)).Replace(VERSION_TAG, string.Format("" + GetCurrentDbVersion()));
		Debug.LogWarning("Diff URL = " + diffUrl);

		string jsondiff = null;
		if(USE_PACKED_DB) {
			//jsondiff = "{ \"@type\":\"com.kiwi.animaltown.db.UserDataWrapper\", \"version\":0,\"LevelSceneData\" :[{\"id\":\"1\",\"name\":\"simple rect\",\"mapLoadType\":\"map\",\"mapModelType\":\"rect_mod\",\"mapFile\":\"testMap 3\",\"mazeSizeX\":\"5\",\"mazeSizeY\":\"5\"}] }";
			jsondiff = packedDb.text;
		} else {
			WWWWrapper diffRequest = new WWWWrapper (diffUrl);
			while(!diffRequest.isDone)
				yield return 0;
			
			diffRequestCompleted = diffRequest.isDone;		
			diffRequestSucceeded = (diffRequest.error == null);
			if (diffRequestSucceeded) {
				jsondiff = diffRequest.text;
				//packedDb.text = jsondiff;
				//UnityEditor.AssetDatabase.SaveAssets();
			}
		}

		if(jsondiff != null)
		{
			Debug.Log("jsondiff "+jsondiff);
			wrapper = JsonConvert.DeserializeObject<T> (jsondiff);
            wrapper.InitNonDiffMarketTable();
			wrapper.InitializeTime();
            GameParamModel.InitGameParams();
            //TODO : MANJEET : MANGODB DOESN'T SET WITH SQL, SO NO LOCAL DB FOR NOW
			//wrapper.InsertIntoDatabase ();
            //TODO : just save the diff in local file for now and reload that at launch time


			//wrapper.PopulateUserAssets(wrapper);
		} else {
			Debug.LogError("Diff Failed");
		}
		Finished();

	}

	public delegate void DiffFinishedAction();
	public static event DiffFinishedAction OnDiffFinished;

	private void Finished() {
         transform.parent.gameObject.BroadcastMessage("DependencyCompleted", ManagerDependency.DATA_LOADED);
		Debug.Log("Diff Finished");
		if(OnDiffFinished != null)
		OnDiffFinished();
	}

	private int GetCurrentDbVersion() {
		KiwiCommonDatabase.IBaseDbHelper dbHelper =  DatabaseManager.GetInstance().GetDbHelper();
		if (dbHelper == null)
			Debug.LogError("DB helper is NULL");
		try {
			MarketVersion dbVersion = dbHelper.QueryObjectById<MarketVersion>(1);
			return dbVersion.version;
		} catch(Exception ex) {
		}
		return 0;
	}
}

}