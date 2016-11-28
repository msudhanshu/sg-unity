using System;

namespace KiwiCommonDatabase
{
	public class DbOpOr: CompoundDbOp
	{
		public DbOpOr(BaseDbOp arg1_, BaseDbOp arg2_, params BaseDbOp[] args) 
			:base(EDbOperators.OR, arg1_, arg2_, args) 
		{
			
		}
	}
}

