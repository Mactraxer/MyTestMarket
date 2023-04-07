using BoxOffices;
using Pool;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Stack = Stacks.Stack;

namespace Buyers
{
	[SelectionBase]
	public class Buyer : MonoBehaviour, IPoolable, IBuyerStateContext
	{
		public event Action<IBuyerStateContext> OnArrived;
		public event Action<IBuyerStateContext> OnReceiveAllProducts;
		public event Action<IPoolable> OnRelease;
		public event Action<IBuyerStateContext> OnEndPay;
		public event Action<Buyer> OnServiced;

		public int ID => GetInstanceID();

		public bool IsActive => gameObject.activeSelf;

		public Transform Transform => transform;

		public Vector3 ShelvingPosition => _shelvingReceivePoint.position;

		public Vector3 BoxOfficePosition => _boxOfficePoint.position;

		public Vector3 ExitPosition => _exitPoint.position;

		public bool ReadyToPay => _readyToPay;

		[SerializeField] private NavMeshAgent _navigationAgent;
		[SerializeField] private Stack _stack;
		[SerializeField] private float _checkArriveDelay;
		[SerializeField] private float _transferProductDelay;
		[SerializeField] private RequirementProduct _requirementProduct;
		[SerializeField] private Collider _collider;

		private Transform _shelvingReceivePoint;
		private Transform _boxOfficePoint;
		private Transform _exitPoint;
		private BoxOffice _boxOffice;
		private BuyerStateMachine _buyerStateMachine;
		private WaitForSeconds _waitForCheckArriveDelay;
		private WaitForSeconds _waitForTransferProductsDelay;
		private Coroutine _transferProductsCoroutine;
		private bool _readyToPay;

		public void Setup(Transform shelvingReceivePoint, Transform boxOfficePoint, Transform exitPoint, BoxOffice boxOffice)
		{
			_shelvingReceivePoint = shelvingReceivePoint;
			_boxOfficePoint = boxOfficePoint;
			_exitPoint = exitPoint;
			_boxOffice = boxOffice;
		}

		public void SetActive(bool active)
		{
			gameObject.SetActive(active);
		}

		public void RouteTo(Vector3 position)
		{
			_navigationAgent.SetDestination(position);
			_navigationAgent.isStopped = false;
			StartCoroutine(CheckArriveLoop(position));
		}

		private IEnumerator CheckArriveLoop(Vector3 position)
		{
			while (_navigationAgent.stoppingDistance < (position - _navigationAgent.transform.position).magnitude)
			{
				yield return _waitForCheckArriveDelay;
			}

			OnArrived?.Invoke(this);
		}

		private void Awake()
		{
			_waitForCheckArriveDelay = new WaitForSeconds(_checkArriveDelay);
			_waitForTransferProductsDelay = new WaitForSeconds(_transferProductDelay);
			_buyerStateMachine = new BuyerStateMachine(this);
			_buyerStateMachine.StatesEnded += BuyerStateMachineStatesEndedHandler;
			_stack.OnChangeCount += StackOnChangeCountHandler;
		}

		private void StackOnChangeCountHandler(int count)
		{
			if (count < _requirementProduct.Count)
			{
				return;
			}

			OnReceiveAllProducts?.Invoke(this);
		}

		private void BuyerStateMachineStatesEndedHandler()
		{
			OnRelease?.Invoke(this);
		}

		public void NextState()
		{
			_buyerStateMachine.Next();
		}

		public void RouteToBoxOffice()
		{
			_buyerStateMachine.Update();
		}

		public void StartPay()
		{
			_transferProductsCoroutine = StartCoroutine(TransferProductsLoop());
		}

		private IEnumerator TransferProductsLoop()
		{
			while (_stack.Count > 0)
			{
				yield return _waitForTransferProductsDelay;
				_stack.Remove(_boxOffice.Stack);
			}

			_boxOffice.OnTakeBox += BoxOfficeOnTakeBoxHandler;
			_boxOffice.ReceiveBoxWithProducts(_stack);
		}

		private void BoxOfficeOnTakeBoxHandler()
		{
			OnServiced?.Invoke(this);
			_boxOffice.OnTakeBox -= BoxOfficeOnTakeBoxHandler;
			NextState();
		}

		public void Active()
		{
			_buyerStateMachine.Start();
		}

		public void Dispose()
		{
			OnRelease?.Invoke(this);
		}

		public void WaitQueue()
		{
			_boxOffice.AddToQueue(this);
		}

		public void FirstInQueue()
		{
			_readyToPay = true;
		}

		public void SetActiveStack(bool active)
		{
			_collider.enabled = active;
		}
	}
}