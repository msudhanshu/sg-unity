using System;
using System.Collections;

namespace KiwiCommonDatabase
{
	public class DbOpNIn : BinaryDbOp
	{
//		public DbOpNIn(object arg1_, KDbQuery arg2_) :base(DbOperators.NOT_IN, arg1_, arg2_) {
//			
//		}
		
		public DbOpNIn(object arg1_, ArrayList arg2_) :base(EDbOperators.NOT_IN, arg1_, arg2_){
			
		}
	}
}

