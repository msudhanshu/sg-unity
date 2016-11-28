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
//using Google.GData.Spreadsheets;

public class CmsSync
{

	//compare this version number with market table/sheet , if less create json from cmsSheet
	private string GetMarketDiffJson(string version) {
		int versionInt = Convert.ToInt32(version);
		string marketdiff = ",";
		string jsonvalue = null;// CmsSync.GetMarketDiff(version);
		if(CmsSync.IsEmpty(jsonvalue)) return "";
		marketdiff+=jsonvalue;
		return marketdiff;
    }

    //compare this version number with market table/sheet , if less create json from cmsSheet
	public static string GetMarketDiff(string version) {
		return "";
		/*
		int versionInt = Convert.ToInt32(version);
		string marketdiff = ",";

		GoogleSpreadsheet gs = GoogleSpreadsheet.gs;
		//loop starts for all sheet
		int totsheet = gs.sheet.Worksheets.Entries.Count;
		for(int i=0;i<1;i++) {
			WorksheetEntry worksheet = (WorksheetEntry)gs.sheet.Worksheets.Entries[i];
			marketdiff += worksheet.XmlName+" :[";

			Dictionary<string, string> sheetvalue = new Dictionary<string, string>();
			int row=2;
			int col =0;
			while(true) { //row
				if( IsEmpty (gs.GetCell(row,1,i) ) ) break;
	            col = 1;
				while(true) { //col
					string title = gs.GetCell(1,col,i);
					if(IsEmpty(title.Trim())) break;
					string value = gs.GetCell(row,col,i);
					sheetvalue.Add(title,value);
					col++;
				}
				
				var serialized = JsonConvert.SerializeObject(sheetvalue);
				DebugDummyServer.Log(serialized);
				marketdiff += serialized +",";
				sheetvalue.Clear();
				row++;
			}

			//remove last ","

			marketdiff += "]" ;
			marketdiff += ",";
		}
			//loop ends for all sheet
			//remove last "," if any
        
        return marketdiff;
*/
    }

	//query in serveruserdatabase
	private string GetUserDiff(string user_id) {
		int user_idInt = Convert.ToInt32(user_id);
		string userdiff = "";
		return "";
	}

	public static bool IsEmpty(string val) {
		if(val==null) return true;
		if(val.Trim() == "") return true;
		return false;
	}
}