using System;

namespace KiwiCommonDatabase
{
	public class DbOpNotEq : BinaryDbOp
	{
		public DbOpNotEq(object arg1_, object arg2_) :base(EDbOperators.NOT_EQUAL, arg1_, arg2_)
		{
			
		}
	}
}

