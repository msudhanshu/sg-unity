using System;

namespace KiwiCommonDatabase
{
	public class DbOpNull : UnaryDbOp
	{
		public DbOpNull(object arg1_) :base(EDbOperators.IS_NULL, arg1_) {

		}
	}
}

