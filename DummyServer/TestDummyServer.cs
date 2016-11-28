//
//  TestDummyServer.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;

public class TestDummyServer : MonoBehaviour
{

	string url = "http://qa6.kiwiup.com/g48/diff?user_id=10&version=0";
	//"http://qa6.kiwiup.com/g48/users/new?email=ms@a.com&device_id=device"

		// Use this for initialization
		void Start ()
		{
		DebugDummyServer.Log("starting .....testserver");
			StartCoroutine(test ());
		DebugDummyServer.Log("start ending .....testserver");
		}

		void Update() {
//		DebugDummyServer.Log("updating .....testserver");
			if(Input.GetKeyDown( KeyCode.A) )
				StartCoroutine(test ());
		}

		IEnumerator test() {
			WWWWrapper wwww = new WWWWrapper(url);
			while(!wwww.isDone)
				yield return 0;
			if(wwww.error != null)
				DebugDummyServer.LogError("wwww.error " + wwww.error);
			else
				DebugDummyServer.Log("wwww.text " + wwww.text);
		}

}

