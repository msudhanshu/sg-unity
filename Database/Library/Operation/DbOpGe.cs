using System;

namespace KiwiCommonDatabase
{
	public class DbOpGe :BinaryDbOp
	{
		public DbOpGe(object arg1_, object arg2_) :base(EDbOperators.GREATER_THAN_EQUAL_TO, arg1_, arg2_) 
		{
		
		}

	}
}

