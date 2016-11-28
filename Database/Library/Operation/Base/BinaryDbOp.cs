using System;

namespace KiwiCommonDatabase
{
	public class BinaryDbOp: BaseDbOp
	{
		protected object arg1, arg2;
		
		protected BinaryDbOp(EDbOperators op_, Object arg1_, Object arg2_) :base(op_)
		{
			arg1 = arg1_;
			arg2 = arg2_;
		}
		
		public object getArg1(){
			return arg1;
		}

		public object getArg2(){
			return arg2;
		}
	}
}

