using System;

namespace KiwiCommonDatabase
{
	public class DbOpEq : BinaryDbOp
	{
		public DbOpEq(Object arg1_, Object arg2_) :base(EDbOperators.EQUALS, arg1_, arg2_)
		{
			
		}
	}
}

