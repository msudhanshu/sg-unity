using System;

namespace KiwiCommonDatabase
{
	public class OrderByValue {
		private string name;
		private bool value;	//For SQL: ASC = true, DESC = false
		
		public OrderByValue(string name_, bool value_){
			this.name = name_;
			this.value = value_;
		}
		
		public string getName(){
			return name;
		}
		
		public bool getValue(){
			return value;
		}
		
	}
}

