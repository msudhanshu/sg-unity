using System;

namespace KiwiCommonDatabase
{
	public class DbOpAnd: CompoundDbOp
	{
		public DbOpAnd(BaseDbOp arg1_, BaseDbOp arg2_, params BaseDbOp[] args) 
			:base(EDbOperators.AND, arg1_, arg2_, args) 
		{
		
		}
	}
}

