namespace Buyers
{
	public abstract class BuyerStateBase
	{
		public abstract BuyerStateType Type { get; }
		public abstract void Enter(IBuyerStateContext buyerStateContext);

		public abstract void Exit(IBuyerStateContext buyerStateContext);
		public abstract void Update(IBuyerStateContext buyerStateContext);
	}
}