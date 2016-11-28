//
//  DummyServer.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;

public class DummyServerManager : Manager<DummyServerManager>  {

    public bool DUMMY_SERVER_ENABLED_TEMP = false;
	public static bool DUMMY_SERVER_ENABLED = false;
    /* Implement this function */
	override public void StartInit() {
        DUMMY_SERVER_ENABLED = DUMMY_SERVER_ENABLED_TEMP;
    }
	
	/* Implement this function */
	override public void PopulateDependencies() {}
		
}

public class DebugDummyServer {
	public static bool DebugTrace = true;
	public static void Log(string s) {
		if(DebugTrace)
			Debug.Log(s);
	}
	public static void LogError(string s) {
		if(DebugTrace)
			Debug.LogError(s);
	}
	public static void LogWarning(string s) {
		if(DebugTrace)
			Debug.LogWarning(s);
	}
}
