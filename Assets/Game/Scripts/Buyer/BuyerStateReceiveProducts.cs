namespace Buyers
{
	public class BuyerStateReceiveProducts : BuyerStateBase
	{
		public override BuyerStateType Type => BuyerStateType.ReceiveProducts;

		public override void Enter(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.OnReceiveAllProducts += OnReceiveAllProductsHandler;
		}

		private void OnReceiveAllProductsHandler(IBuyerStateContext contex)
		{
			contex.OnReceiveAllProducts -= OnReceiveAllProductsHandler;
			contex.NextState();
		}

		public override void Exit(IBuyerStateContext buyerStateContext)
		{
			
		}
	}
}