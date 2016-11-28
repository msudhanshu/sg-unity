using System;
using System.Collections;

namespace KiwiCommonDatabase
{
	public class DbOpIn : BinaryDbOp
	{
//		public DbOpIn(object arg1_, KDbQuery arg2_) :base(DbOperators.IN, arg1_, arg2_) {
//
//		}
		
		public DbOpIn(object arg1_, ArrayList arg2_) :base(EDbOperators.IN, arg1_, arg2_){

		}
	}
}

