namespace Buyers
{
	public class BuyerStateReceiveProducts : BuyerStateBase
	{
		public override BuyerStateType Type => BuyerStateType.ReceiveProducts;

		public override void Enter(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.OnReceiveAllProducts += OnReceiveAllProductsHandler;
			buyerStateContext.SetActiveStack(true);
		}

		private void OnReceiveAllProductsHandler(IBuyerStateContext contex)
		{
			contex.OnReceiveAllProducts -= OnReceiveAllProductsHandler;
			contex.NextState();
		}

		public override void Update(IBuyerStateContext buyerStateContext)
		{
			
		}
	}
}