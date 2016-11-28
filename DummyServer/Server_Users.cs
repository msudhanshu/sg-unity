//
//  Server_Users.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Server_Users : Server_Controller
{
	public string New(Dictionary<string,string> param) {
		for(int i=0;i <1000;i++) {
			DebugDummyServer.Log("server working");
		}
		DebugDummyServer.Log("server working done. " );
		DebugDummyServer.Log("Param="+ param["email"] + ","+param["device_id"]);
		return "new user created";
	}
}