using System;

namespace KiwiCommonDatabase
{
	public class DbOpLike : BinaryDbOp
	{
		public DbOpLike(object arg1_, object arg2_) :base(EDbOperators.LIKE, arg1_, arg2_) 
		{
			
		}
	}
}

