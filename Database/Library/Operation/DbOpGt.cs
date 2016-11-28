using System;

namespace KiwiCommonDatabase
{
	public class DbOpGt :BinaryDbOp
	{
		public DbOpGt(object arg1_, object arg2_) :base(EDbOperators.GREATER_THAN, arg1_, arg2_) 
		{
			
		}
		
	}
}
