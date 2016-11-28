using System;

namespace KiwiCommonDatabase
{
	public class DbOpLe : BinaryDbOp
	{
		public DbOpLe(object arg1_, object arg2_) :base(EDbOperators.LESS_THAN_EQUAL_TO, arg1_, arg2_) 
		{
			
		}
	}
}

