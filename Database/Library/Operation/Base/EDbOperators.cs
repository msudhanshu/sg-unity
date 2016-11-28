using System;
using UnityEngine;

namespace KiwiCommonDatabase
{
	public enum EDbOperators 
	{
		GREATER_THAN,	//Binary operators
		LESS_THAN,
		GREATER_THAN_EQUAL_TO,
		LESS_THAN_EQUAL_TO,
		LIKE,
		NOT_LIKE,
		EQUALS,
		NOT_EQUAL,

		IS_NOT_NULL,	//Unary operators
		IS_NULL,

		IN,				//In list of objects
		NOT_IN,

		AND,			//Compound operations like where.or(where.notNull("test_1"), whereIsNull("test_2"))
		OR
	}


	public class DbOperatorExtension{
		public static string getOpCode(EDbOperators op){
			switch(op){

				case EDbOperators.GREATER_THAN:
					return ">";
					
				case EDbOperators.LESS_THAN:
					return "<";
					
				case EDbOperators.GREATER_THAN_EQUAL_TO:
					return ">=";
					
				case EDbOperators.LESS_THAN_EQUAL_TO:
					return "<=";
					
				case EDbOperators.LIKE:
					return "like";
					
				case EDbOperators.NOT_LIKE:
					return "not like";
					
				case EDbOperators.EQUALS:
					return "=";
					
				case EDbOperators.NOT_EQUAL:
					return "!=";
					
				case EDbOperators.IS_NOT_NULL:
					return "is not null";
					
				case EDbOperators.IS_NULL:
					return "is null";
					
				case EDbOperators.AND:
					return "and";
					
				case EDbOperators.OR:
					return "or";
					
				case EDbOperators.IN:
					return "in";
					
				case EDbOperators.NOT_IN:
					return "not in";
					
//				case DbOperators.AND_CONJOIN:
//					return "and_conjoin";
//					
//				case DbOperators.OR_CONJOIN:
//					return "or_conjoin";
					
				default:
					return "";
				
			}
		}
	}
}

