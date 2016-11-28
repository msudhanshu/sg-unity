using System;

namespace KiwiCommonDatabase
{
	public class DbOpLt : BinaryDbOp
	{
		public DbOpLt(object arg1_, object arg2_) :base(EDbOperators.LESS_THAN, arg1_, arg2_) 
		{
			
		}
	}
}

