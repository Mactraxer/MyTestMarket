namespace Buyers
{
	public class BuyerStatePay : BuyerStateBase
	{
		public override BuyerStateType Type => BuyerStateType.Pay;

		public override void Enter(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.StartPay();
			buyerStateContext.OnEndPay += BuyerOnEndPayHandler;
		}

		private void BuyerOnEndPayHandler(IBuyerStateContext context)
		{
			context.OnEndPay -= BuyerOnEndPayHandler;
			context.NextState();
		}

		public override void Update(IBuyerStateContext buyerStateContext)
		{
			
		}
	}
}