using System;

namespace KiwiCommonDatabase
{
	public class DbOpNotNull : UnaryDbOp
	{

		public DbOpNotNull(object arg1_) :base(EDbOperators.IS_NOT_NULL, arg1_) {
			
		}
	}
}


