namespace Buyers
{
	public class BuyerStateRouteToExit : BuyerStateBase
	{
		public override BuyerStateType Type => BuyerStateType.Exit;

		public override void Enter(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.OnArrived += BuyerOnArrivedHandler;
			buyerStateContext.RouteTo(buyerStateContext.ExitPosition);
		}

		private void BuyerOnArrivedHandler(IBuyerStateContext context)
		{
			context.OnArrived -= BuyerOnArrivedHandler;
			context.NextState();
		}

		public override void Exit(IBuyerStateContext buyerStateContext)
		{
			
		}

		public override void Update(IBuyerStateContext buyerStateContext)
		{
			
		}
	}
}