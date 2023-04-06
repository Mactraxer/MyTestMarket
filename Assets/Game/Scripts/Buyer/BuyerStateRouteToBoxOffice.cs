﻿namespace Buyers
{
	public class BuyerStateRouteToBoxOffice : BuyerStateBase
	{
		public override BuyerStateType Type => BuyerStateType.BoxOffice;

		public override void Enter(IBuyerStateContext buyerStateContext)
		{
			buyerStateContext.OnArrived += BuyerOnArrivedHandler;
		}

		private void BuyerOnArrivedHandler(IBuyerStateContext context)
		{
			context.OnArrived -= BuyerOnArrivedHandler;
			context.NextState();
		}

		public override void Exit(IBuyerStateContext buyerStateContext)
		{
			
		}
	}
}