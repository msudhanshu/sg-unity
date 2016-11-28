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
    public class MarketDataWrapper {

    	public List<QuestionModel> questions;
        public List<PackageModel> packages;
        public List<CurrencyModel> currencies;
        public List<GameParamModel> gameParams;
        public List<EffectModel> effects;

    	public int version;


    	public static bool IsDbTable(FieldInfo field) {
    		foreach (object attribute in field.GetCustomAttributes(true))
    		{
    			if (attribute is DbTableAttribute) return true;
    		}
    		return false;
    	}

    	public void InsertIntoDatabase() {
    		DatabaseManager dbManager = DatabaseManager.GetInstance();
            dbManager.InsertMarketTable<QuestionModel>(questions);
            dbManager.InsertMarketTable<PackageModel>(packages);
    		dbManager.UpdateMarketVersion (version);
    	}

        public void InitNonDiffMarketTable() {
            currencies = CurrencyModel.InitSelfNonDiffMarketTable();
            gameParams = GameParamModel.InitSelfNonDiffMarketTable();
            effects = EffectModel.InitSelfNonDiffMarketTable();
        }

    }

}