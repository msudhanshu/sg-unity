using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleSQL;

namespace KiwiCommonDatabase
{
	/*
	 * Base class for all the database model classes
	 */
	public class BaseDbModel
	{
		protected Dictionary<string, object[]> relationsMap = null;	//Array Index 0=fieldName, 1=PropertyInfo, 2=Value

		public BaseDbModel ()
		{
		}

		protected object getRelationalFieldValue(string relationFieldName, string basicFieldName){
			if(relationsMap == null){
				relationsMap = new Dictionary<string, object[]>();
			}
			object relationFieldValue = null;
			object[] fieldValue = relationsMap.ContainsKey(relationFieldName) ? relationsMap[relationFieldName] : null;
			if (fieldValue != null && fieldValue.Length > 0) {
				//Already exists, fetch from memory
			} else {
				fieldValue = new object[3];

				PropertyInfo fieldPropertyInfo = this.GetType().GetProperty (basicFieldName, BindingFlags.Public | BindingFlags.Instance);
				object basicFieldValue = fieldPropertyInfo.GetValue(this, null);

				PropertyInfo relFieldPropertyInfo = this.GetType().GetProperty (relationFieldName, BindingFlags.Public | BindingFlags.Instance);
				string relationClassName = relFieldPropertyInfo.PropertyType.ToString();

				Debug.Log("relationClassName=" + relationClassName + ", relationFieldName=" + relationFieldName + ", basicFieldName=" + basicFieldName + ", basicFieldValue=" + basicFieldValue);
				relationFieldValue = DatabaseManager.GetInstance ().GetDbHelper ().InvokeGenericMethodByReflection (
					relationClassName, "QueryObjectById", new object[]{basicFieldValue});

				fieldValue[0] = basicFieldName;
				fieldValue[1] = fieldPropertyInfo;
				fieldValue[2] = relationFieldValue;
			}

//			Debug.Log("relationFieldValue = " + (relationFieldValue != null ? relationFieldValue.ToString() : "null"));

			return fieldValue [2];
		}

		/**
		 *  newRelationalFieldObject is the db object with only id field populated after de-serialization, as comes from diff data
		 **/
		protected void setRelationalFieldValue(string relationFieldName, string basicFieldName, object newRelationalFieldObject){
			if(relationsMap == null){
				relationsMap = new Dictionary<string, object[]>();
			}

			//Query by id field of new relational object
			PropertyInfo idFieldPropertyInfo = GetType ().GetProperty ("id", BindingFlags.Public | BindingFlags.Instance);
			object basicFieldValue = idFieldPropertyInfo.GetValue(newRelationalFieldObject, null);
			PropertyInfo fieldPropertyInfo = null;

			PropertyInfo relFieldPropertyInfo = this.GetType().GetProperty (relationFieldName, BindingFlags.Public | BindingFlags.Instance);
			string relationClassName = relFieldPropertyInfo.PropertyType.ToString();
			
			object[] fieldValue = relationsMap[relationFieldName];
			if (fieldValue != null && fieldValue.Length > 0) {
				fieldPropertyInfo = (PropertyInfo) fieldValue[1];
				object relationDbObject = DatabaseManager.GetInstance ().GetDbHelper ().InvokeGenericMethodByReflection (
					relationClassName, "QueryObjectById", new object[]{basicFieldValue});

				//Update the db object in memory also with all fields populated
				fieldValue[2] = relationDbObject;
			} else {
				fieldValue = new object[3];
				object relationDbObject = DatabaseManager.GetInstance ().GetDbHelper ().InvokeGenericMethodByReflection (
					relationClassName, "QueryObjectById", new object[]{basicFieldValue});

				fieldValue[0] = basicFieldName;
				fieldValue[1] = fieldPropertyInfo;
				//Update the db object in memory also with all fields populated
				fieldValue[2] = relationDbObject;
			}

			//Update the value of the basicField type also
			fieldPropertyInfo.SetValue(this, basicFieldValue, null);
		}


		public override bool Equals(System.Object obj){
			if (obj != null && obj is BaseDbModel) {
				//Compare by id field value of database objects
				PropertyInfo propertyInfoObj1 = this.GetType ().GetProperty ("id", BindingFlags.Public | BindingFlags.Instance);
				object idValueObj1 = propertyInfoObj1.GetValue(this, null);

				//Debug.Log("Object 1 id value = " + idValueObj1);

				BaseDbModel obj2 = (BaseDbModel) obj;
				PropertyInfo propertyInfoObj2 = obj2.GetType ().GetProperty ("id", BindingFlags.Public | BindingFlags.Instance);
				object idValueObj2 = propertyInfoObj2.GetValue(obj2, null);

				//Debug.Log("Object 2 id value = " + idValueObj2);

				return idValueObj1.Equals(idValueObj2);
			} else {
				//Error: Should pass only BaseDbModel type to check for equality; Return default implementation
				return base.Equals(obj);
			}
		}


		public override int GetHashCode()
		{
			int prime = 31;
			int result = 1;
			
			PropertyInfo propertyInfoObj = this.GetType ().GetProperty ("id", BindingFlags.Public | BindingFlags.Instance);
			object idValueObj = propertyInfoObj.GetValue(this, null);
			
			result = prime * result + idValueObj.GetHashCode();
			return result;
		}


		public static bool operator ==(BaseDbModel a, System.Object b)
		{

			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}
			
			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}
			
			if (a is BaseDbModel && b is BaseDbModel) {
				//Compare by id field value of database objects
				PropertyInfo propertyInfoObj1 = a.GetType ().GetProperty ("id", BindingFlags.Public | BindingFlags.Instance);
				object idValueObj1 = propertyInfoObj1.GetValue (a, null);

				Debug.Log ("==Override: 1 id value = " + idValueObj1);

				BaseDbModel obj2 = (BaseDbModel)b;
				PropertyInfo propertyInfoObj2 = b.GetType ().GetProperty ("id", BindingFlags.Public | BindingFlags.Instance);
				object idValueObj2 = propertyInfoObj2.GetValue (b, null);

//				Debug.Log ("==Override: Object 2 id value = " + idValueObj2);
//
//				Debug.Log ("==Override?" + (idValueObj1 == idValueObj2));
//				Debug.Log ("==Override? As Equals: " + (idValueObj1.Equals(idValueObj2)));

				return idValueObj1.Equals(idValueObj2);
			} else {

				return a.Equals(b);
			}


		}
		
		public static bool operator !=(BaseDbModel a, System.Object b)
		{
			return !(a == b);
		}






	}
}

