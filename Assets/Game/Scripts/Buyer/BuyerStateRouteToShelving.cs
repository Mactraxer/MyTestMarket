namespace Buyers
{
	public class BuyerStateRouteToShelving : BuyerStateBase
	{
		public override BuyerStateType Type => BuyerStateType.Shelving;

		public override void Enter(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.OnArrived += BuyerOnArrivedHandler;
			buyerStateContext.RouteTo(buyerStateContext.ShelvingPosition);
		}

		private void BuyerOnArrivedHandler(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.OnArrived -= BuyerOnArrivedHandler;
			buyerStateContext.NextState();
		}

		public override void Update(IBuyerStateContext buyerStateContext)
		{
			
		}
	}
}