namespace Buyers
{
	public class BuyerStateRouteToShelving : BuyerStateBase
	{
		public override BuyerStateType Type => BuyerStateType.Shelving;

		public override void Enter(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.RouteTo(buyerStateContext.ShelvingPosition);
			buyerStateContext.OnArrived += BuyerOnArrivedHandler;
		}

		private void BuyerOnArrivedHandler(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.OnArrived -= BuyerOnArrivedHandler;
			buyerStateContext.NextState();
		}

		public override void Exit(IBuyerStateContext buyerStateContext)
		{
			
		}
	}
}