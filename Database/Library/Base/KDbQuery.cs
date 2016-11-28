using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;

namespace KiwiCommonDatabase
{
	public class KDbQuery<T> where T:BaseDbModel {
		protected T clazz;
		protected List<BaseDbOp> andedOperations = null;
		
		protected KDbQueryAttribute dbQueryAttribute = null;

		public KDbQuery(){

		}

		public KDbQuery(BaseDbOp[] andedOps_)  :this(){
			andedOperations = andedOps_.OfType<BaseDbOp> ().ToList ();
		}
		
		public KDbQuery(List<BaseDbOp> andedOps_) :this(){
			andedOperations = andedOps_;
		}

		public KDbQuery(BaseDbOp dbOp_) :this(){
			andedOperations = new List<BaseDbOp>(){dbOp_};
		}

		public string getDbClass(){
			return typeof(T).ToString ();
		}
		
		public BaseDbOp[] getAndedDbOperationsArray(){
			if(andedOperations != null){
				return andedOperations.ToArray();
			}
			return null;
		}
		
		public List<BaseDbOp> getAndedDbOperationsList(){
			return andedOperations;
		}
		
		public void setQueryAttribute(KDbQueryAttribute attribute){
			dbQueryAttribute = attribute;
		}
		
		public KDbQueryAttribute getQueryAttribute(){
			return dbQueryAttribute;
		}
		
		
	}

}

