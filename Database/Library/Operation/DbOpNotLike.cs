using System;

namespace KiwiCommonDatabase
{
	public class DbOpNotLike : BinaryDbOp
	{

		public DbOpNotLike(object arg1_, object arg2_) :base(EDbOperators.NOT_LIKE, arg1_, arg2_) 
		{
			
		}
	}
}
