using System;

namespace Buyers
{
	public class BuyerStateMachine
	{
		public event Action StatesEnded;

		private readonly BuyerStateRouteToShelving _buyerStateRouteToShelving = new();
		private readonly BuyerStateReceiveProducts _buyerStateReceiveProducts = new();
		private readonly BuyerStateRouteToBoxOffice _buyerStateRouteToBox = new();
		private readonly BuyerStateWaitQueue _buyerStateWaitQueue = new();
		private readonly BuyerStatePay _buyerStatePay = new();
		private readonly BuyerStateRouteToExit _buyerStateRouteToExit = new();

		private readonly IBuyerStateContext _context;

		private BuyerStateBase _state;

		public BuyerStateMachine(IBuyerStateContext context)
		{
			_context = context;
		}

		public void Next()
		{
			_state.Exit(_context);

			switch(_state.Type)
			{
				case BuyerStateType.Shelving:
					_state = _buyerStateReceiveProducts;
					_state.Enter(_context);
					break;
				case BuyerStateType.ReceiveProducts:
					_state = _buyerStateRouteToBox;
					_state.Enter(_context);
					break;
				case BuyerStateType.BoxOffice:
					_state = _buyerStateWaitQueue;
					_state.Enter(_context);
					break;
				case BuyerStateType.WaitQueue:
					_state = _buyerStatePay;
					_state.Enter(_context);
					break;
				case BuyerStateType.Pay:
					_state = _buyerStateRouteToExit;
					_state.Enter(_context);
					break;
				case BuyerStateType.Exit:
					StatesEnded?.Invoke();
					break;
				default:
					break;
			}

		}

		public void Start()
		{
			_state = _buyerStateRouteToShelving;
			_buyerStateRouteToShelving.Enter(_context);
		}

		public void Update()
		{
			_state.Update(_context);
		}
	}
}