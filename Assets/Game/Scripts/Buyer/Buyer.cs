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

		public int ID => GetInstanceID();

		public bool IsActive => gameObject.activeSelf;

		public Transform Transform => transform;

		public Vector3 ShelvingPosition => _shelvingReceivePoint.position;

		public Vector3 BoxOfficePosition => _boxOfficePoint.position;

		[SerializeField] private NavMeshAgent _navigationAgent;
		[SerializeField] private Stack _stack;
		[SerializeField] private float _checkArriveDelay;
		[SerializeField] private float _transferProductDelay;
		[SerializeField] private RequirementProduct _requirementProduct;

		private Transform _shelvingReceivePoint;
		private Transform _boxOfficePoint;
		private Transform _exitPoint;
		private BoxOffice _boxOffice;
		private BuyerStateMachine _buyerStateMachine;
		private WaitForSeconds _waitForCheckArriveDelay;
		private WaitForSeconds _waitForTransferProductsDelay;
		private Coroutine _transferProductsCoroutine;

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
			StartCoroutine(CheckArriveLoop());
		}

		private IEnumerator CheckArriveLoop()
		{
			while (_navigationAgent.stoppingDistance > (_navigationAgent.pathEndPosition - _navigationAgent.transform.position).magnitude)
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

			_buyerStateMachine.Next();
		}

		private void BuyerStateMachineStatesEndedHandler()
		{
			OnRelease?.Invoke(this);
		}

		public void NextState()
		{
			_buyerStateMachine.Next();
		}

		public void ReadyToPay()
		{
			_buyerStateMachine.Next();
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
			_boxOffice.OnTakeBox -= BoxOfficeOnTakeBoxHandler;
			NextState();
		}

		public void Active()
		{
			_buyerStateMachine.Start();
		}
	}
}