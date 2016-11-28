using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

public class Singleton<T> where T : UnityEngine.Object {
	private static T sInstance;
	
	public static void ClearInstance() {
		sInstance = null;
	}
	
	public static T instance {
		get {
			if(sInstance == null) {
				sInstance = (T)Object.FindObjectOfType(typeof(T));
			}
			return sInstance;
		}
		
		set {
			sInstance = value;
		}
	}

	public static T instanceNoServer {
		get {
			if(sInstance == null) {
				sInstance = (T)Object.FindObjectOfType(typeof(T));
				#if SINGLETON_PRINTS
				if (sInstance == null)
				{
					Debug.Log("Could not find " + typeof(T).Name);
				}
				#endif
			}
			return sInstance;
		}
	}
	
	public static T instanceNoFind {
		get {
			if(sInstance == null) {
				#if SINGLETON_PRINTS
				Debug.Log("Could not find " + typeof(T).Name);
				#endif
			}
			return sInstance;
		}
		
		set {
			sInstance = value;
		}
	}
	
	public static T instanceManagement(string assetPath)
	{
		#if UNITY_EDITOR
		if (sInstance == null)
		{
			sInstance = (T)Object.FindObjectOfType(typeof(T));
			if (sInstance == null)
			{
				if (Application.isPlaying)
				{
					EditorUtility.DisplayDialog("Error", "You must put a " + assetPath + " in your scene", "OK");
					UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Play");
				}
				
				GameObject o = (GameObject)UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPath);
				if (o == null)
				{
					Debug.LogError("expecting " + assetPath + " to exist");
				}
				
				UnityEditor.PrefabUtility.InstantiatePrefab(o);
				
				sInstance = (T)Object.FindObjectOfType(typeof(T));
				
				if (sInstance == null)
				{
					Debug.LogError("expecting to have created an instance of " + assetPath);
				}
			}
		}
		return sInstance;
		#else
		if (sInstance == null)
		{
//			sInstance = (T)Object.FindObjectsOfType(typeof(T));
			
			if (sInstance == null)
			{
				Debug.LogError("You need to have an instance of " + typeof(T).Name + " in your scene");
			}
		}
		return sInstance;
		#endif
	}
	
	public static T instanceTodoNull {
		get {
			return null;
		}
	}
}


public class Utility {
	
	public static  string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		
		return hashString.PadLeft(32, '0');
	}

	public static System.DateTime FromUnixTime(long unixTime)
	{
		System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return epoch.AddSeconds(unixTime);
	}

	public static long ToUnixTime(System.DateTime date)
	{
		System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return Convert.ToInt64((date - epoch).TotalSeconds);
	}

    public static string ToDateTimeString(long epocTime) {
        return ToDateTimeString(FromUnixTime(epocTime));
    }

    public static string ToDateTimeString(System.DateTime date) {
        //Debug.Log("day="+date.Day+", hr="+date.Hour+", min="+date.Minute+", sec="+date.Second);
        int days = date.Day;
        int hours = (int)date.Hour % 24;
        int mins = (int) (date.Minute) % 60 ;
        int seconds = (int) (date.Second) % 60 ;

        //Debug.Log(days+" days:"+hours+" H:"+mins+": M"+seconds+":S");

        if(days>0) {
            return days+" days:"+hours+" Hr";
        } else if(hours > 0)
            return hours+" H:"+mins+": M"+seconds+":S";
        else 
            return mins+": M"+seconds+":S";
    }


	public static List<string> StringToList(string s) {
		List<string> list = new List<string>();
		if (s == null)
			return list;

		string[] elements = s.Split(ServerConfig.commaDelimiters);

		for (int i = 0; i < elements.Length; i++) {
			if (elements[i] != null && !elements[i].Equals(""))
				list.Add(elements[i]);
		}
		return list;
	}

	public static bool StringEquals(string a, string b, bool IgnoreCase=true) {
		if(IgnoreCase)
			return string.Equals(a, b, System.StringComparison.InvariantCultureIgnoreCase);
		else
			return string.Equals(a, b, System.StringComparison.InvariantCulture);
		//return string.Compare(a, b, true) == 0;
	}

	public static bool StringEmpty(string a) {
		return a==null || string.Equals(a.Trim(), "", System.StringComparison.InvariantCultureIgnoreCase);
		//return string.Compare(a, b, true) == 0;
	}

	public static string StringToLower(string a) {
			return a.ToLower();
	}

	public static long GetServerTime() {
		return ServerConfig.serverTimeAtSessionStart + (Utility.ToUnixTime(System.DateTime.Now) - ServerConfig.localTimeAtSessionStart);
	}

}




public class Util
{
	public const float HOURS_TO_SECONDS = 60.0f * 60.0f;
    public const float DAYS_TO_SECONDS = 24.0f * 60.0f * 60.0f;
	public static Transform SearchHierarchyForBone(Transform current, string name)   
	{
		if (current.name == name)
			return current;
		
		for (int i = 0; i < current.GetChildCount(); ++i)
		{
			Transform found = SearchHierarchyForBone(current.GetChild(i), name);
			if (found != null)
				return found.transform;
		}
		
		return null;
	}
	
	public static Transform SearchHierarchyForBoneAlt(Transform current, string name)   
	{
		Transform[] allChildren = current.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			if (child.gameObject.name == name || child.name == name) {
				return child;
			}
		}
		
		return null;
	}
	
	public static void ChangeLayer(Transform current, int layer)   
	{
		current.gameObject.layer = layer;
		Transform[] allChildren = current.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			child.gameObject.layer = layer;
		}
	}

	static public void SafeDestroy(Object o)
	{
		if (o == null) return;
		
		if (Application.isPlaying)
		{
			Object.Destroy(o);
		}
		else
		{
			Debug.Log("SafeDestroy calling DestroyImmediate on " + o.name);
			Object.DestroyImmediate(o);
		}
		
		// Util.SafeDestroy( o ); ?
	}
	
	static public void SafeDestroy(Material m)
	{
		if (m == null) return;
		SafeDestroy((Object)m);
	}
	
	static public void SafeDestroy(Shader s)
	{
		if (s == null) return;
		SafeDestroy((Object)s);
	}
	
	static public void SafeDestroy(GameObject o)
	{
		if (o == null) return;
		SafeDestroy((Object)o);
	}
	
	static public void SetActive( GameObject o, bool on )
	{
		SetActive( o, on, true );
	}
	
	static public void SetActive( GameObject o, bool on, bool enableParentFirst )
	{
		if (o == null || o.transform == null) return;
		
		if( enableParentFirst )
			o.SetActive( on );
		
		for (int i = 0; i < o.transform.childCount; i++)
		{
			Transform child = o.transform.GetChild(i);
			if (child == null) continue;
			if (child.gameObject == null) continue;
			
			SetActive( child.gameObject, on, enableParentFirst );
		}
		
		if( !enableParentFirst )
			o.SetActive( on );
	}
	
	[Obsolete("Please use Util.SetActive() instead")]
	static internal void SetActiveRecursivelyWithoutLeakingMemory( GameObject o, bool on )
	{
		SetActive( o, on );
	}
	
	static internal void SetLayerRecursively(GameObject o, int layer)
	{
		if (o == null || o.transform == null) return;
		
		o.layer = layer;
		
		for (int i = 0; i < o.transform.childCount; i++)
		{
			Transform child = o.transform.GetChild(i);
			if (child == null) continue;
			if (child.gameObject == null) continue;
			
			SetLayerRecursively(child.gameObject, layer);
		}
	}

	public static GameObject InstantiatePrefab(string prefabName, bool pool = false) {
		GameObject assetCatPrefab = null;
		GameObject goPrefab = (GameObject) Resources.Load(prefabName, typeof(GameObject));
		if(goPrefab!=null) {
			if(pool) {
				Transform t = Pool.Instantiate(goPrefab);
				if(t!=null) assetCatPrefab= t.gameObject;
			}else
				assetCatPrefab =  (GameObject)GameObject.Instantiate(goPrefab);
			return assetCatPrefab;
		} 
		Debug.LogError("Prefab could not be created. PrefabName="+prefabName);
		return null;
	
	}
}

