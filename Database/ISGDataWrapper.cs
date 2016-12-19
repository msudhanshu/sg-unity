using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using KiwiCommonDatabase;
using SgUnity;
namespace KiwiCommonDatabase
{
    
    [System.Serializable]
	public abstract class ISGDataWrapper {
		public long serverEpochTimeAtSessionStart;

		public abstract void InsertIntoDatabase ();
		public abstract void InitNonDiffMarketTable ();

		public void InitializeTime() {
			ServerConfig.serverTimeAtSessionStart = serverEpochTimeAtSessionStart;
			ServerConfig.localTimeAtSessionStart = Utility.ToUnixTime(System.DateTime.Now);
		}
    }

}