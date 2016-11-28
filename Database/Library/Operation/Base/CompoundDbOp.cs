using System;

namespace KiwiCommonDatabase
{
	/**
	 * Compound operations: This is to handle compound operations like a AND b AND (c OR d). 
	 * We pass and array of dbOperations in a array, which are ANDed by default.
	 * For OR operations, we could pass c OR d as a compound operation. 
	 * Here c in itself could be compound operation (e AND f).
	 * 
	 * @author vineetverma
	 */
	public class CompoundDbOp: BaseDbOp
	{
		protected BaseDbOp arg1, arg2;
		private BaseDbOp[] operands = null;
		
		/*
	 	 * Note that the same, single dboperator (AND for DbOpAnd, OR for DbOpOr) is applied to 
	 	 * all the operands passed as args in a single compound dbop.
	 	 */
		protected CompoundDbOp(EDbOperators op_, BaseDbOp arg1_, BaseDbOp arg2_, params BaseDbOp[] args): base(op_){
			arg1 = arg1_;
			arg2  = arg2_;
			operands = new BaseDbOp[2 + args.Length];
			operands [0] = arg1_;
			operands [1] = arg2_;
			if(args != null && args.Length > 0){
				Array.Copy (args, 0, operands, 2, args.Length);
			}
		}
		
		public BaseDbOp getArg1(){
			return arg1;
		}
		
		public BaseDbOp getArg2(){
			return arg2;
		}
		
		public BaseDbOp[] getCompoundOperands(){
			return operands;
		}

	}
}

