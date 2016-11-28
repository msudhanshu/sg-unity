using System;
namespace KiwiCommonDatabase
{
	public class UnaryDbOp: BaseDbOp
	{
		protected object arg1;

		protected UnaryDbOp(EDbOperators op_, Object arg1_) :base(op_)
		{
			arg1 = arg1_;
		}
		
		public object getArg1(){
			return arg1;
		}
	}
}

