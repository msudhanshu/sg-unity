//
//  Server_Index.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SampleClassLibrary;
using Newtonsoft.Json;
using System.Linq;

public class Server_Index : Server_Controller
{
	string jsonDiff = null;

	public void Diff(Dictionary<string,string> param) {
		DebugDummyServer.Log("1: Param="+ param["user_id"] + ","+param["version"]);
		//DummyServerManager.GetInstance().StartCoroutine(DiffCoroutine( param["user_id"] ,param["version"] ));

		DiffCoroutine( param["user_id"] ,param["version"]);

		DebugDummyServer.Log("2 : Param="+ param["user_id"] + ","+param["version"]);
	}

	//	IEnumerator DiffCoroutine(string user_id, string version) {
	void DiffCoroutine(string user_id, string version) {

		jsonDiff = "{ ";
		jsonDiff += "\"@type\"";
		jsonDiff += ":\"com.kiwi.animaltown.db.UserDataWrapper\", \"version\":"+version;
		//yield return DummyServerManager.GetInstance().StartCoroutine( GetMarketDiff(user_id) );
		//GetMarketDiff(user_id);
		jsonDiff += GetMarketDiff(user_id);
		jsonDiff += GetUserDiff(version);
		jsonDiff += " }";
		if(serverResponseListner!=null)
			serverResponseListner.OnComplete(jsonDiff);

		return;
		//yield return 0;
	}

	//compare this version number with market table/sheet , if less create json from cmsSheet
	public string GetMarketDiff(string version) {
		int versionInt = Convert.ToInt32(version);
		string marketdiff = "";
		DebugDummyServer.Log("Starting reading spreadsheet");

		//marketdiff = GetMarketDiffFromGS(version);
		//loop ends for all sheet
		//remove last "," if any
		return marketdiff;
	//	yield return 0;
	}



	/*
	using Google.GData.Spreadsheets;
	private string GetMarketDiffFromGS(string versoin) {
		string marketdiff = "";
		GoogleSpreadsheet gs = GoogleSpreadsheet.gs;// new GoogleSpreadsheet();
		//while(!gs.IsReady()) yield return null;
		//loop starts for all sheet
		int totsheet = gs.sheet.Worksheets.Entries.Count;
		for(int i=0;i<totsheet;i++) {
			WorksheetEntry worksheet = (WorksheetEntry)gs.sheet.Worksheets.Entries[i];
			if ( DatabaseManager.DB_TABLES.Any(worksheet.Title.Text.Contains) )//worksheet.Title.Text) {
			{
				marketdiff = ",";
				DebugDummyServer.Log("Reading : "+worksheet.Title.Text);
				marketdiff += "\""+worksheet.Title.Text +"\" :[";
				
				Dictionary<string, string> sheetvalue = new Dictionary<string, string>();
				int row=2;
				int col =0;
				bool flagAllRowDone = false;
				while(true) { //row
					col = 1;
					while(true) { //col
						string title = gs.GetCell(1,col,i);
						if(CmsSync.IsEmpty(title)) break;
						//	yield return null;
						string value = gs.GetCell(row,col,i);
						
						if(col==1 && CmsSync.IsEmpty(value) ) {flagAllRowDone=true; break;}
						
						sheetvalue.Add(title,value);
						col++;
						//	yield return null;
					}
					if(flagAllRowDone) break;
					var serialized = JsonConvert.SerializeObject(sheetvalue);
					DebugDummyServer.Log(serialized);
					marketdiff += serialized+",";
					sheetvalue.Clear();
					row++;
				}
				marketdiff = RemoveLastDelimeterIfAny(marketdiff,',');
				//remove last ","
				
				marketdiff += "]" ;
			}
		}
	}
*/

	private string RemoveLastDelimeterIfAny(string s, char delimeter) {
		//if(s.LastIndexOf(delimeter) == s.Length-1)
		{
			return s.Remove(s.Length-1);
		}
	}
    
	//query in serveruserdatabase
	private string GetUserDiff(string user_id) {
		int user_idInt = Convert.ToInt32(user_id);
		string userdiff = "";
		return "";
	}
}