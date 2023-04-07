namespace Buyers
{
	public class BuyerStateWaitQueue : BuyerStateBase
	{
		public override BuyerStateType Type => BuyerStateType.WaitQueue;

		public override void Enter(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.WaitQueue();
		}

		public override void Exit(IBuyerStateContext buyerStateContext)
		{
		}

		public override void Update(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.OnArrived += BuyerOnArrivedHandler;
			buyerStateContext.RouteTo(buyerStateContext.BoxOfficePosition);
		}

		private void BuyerOnArrivedHandler(IBuyerStateContext context)
		{
			if(context.ReadyToPay)
			{
				context.OnArrived -= BuyerOnArrivedHandler;
				context.StartPay();
			}
		}
	}
}