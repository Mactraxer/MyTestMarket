using Buyers;
using Pool;
using Stacks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoxOffices
{
	public class BoxOffice : MonoBehaviour
	{
		public event Action<Buyer> FirtOnQueue;

		public event Action OnTakeBox;

		[SerializeField] private Stack _stack;
		[SerializeField] private ActionArea _actionArea;
		[SerializeField] private BoxProducts _boxProductsPrefab;

		private Queue<Buyer> _buyersQueue = new();
		private bool _isReadyToServiceBuyer = false;

		public Stack Stack => _stack;

		private void Start()
		{
			_actionArea.OnStopAction += ActionAreaOnStopActionHandler;
			_actionArea.OnStartAction += ActionAreaOnStartActionHandler;
		}

		private void OnDisable()
		{
			_actionArea.OnStopAction -= ActionAreaOnStopActionHandler;
			_actionArea.OnStartAction -= ActionAreaOnStartActionHandler;
		}

		private void ActionAreaOnStartActionHandler(Stack _)
		{
			_isReadyToServiceBuyer = true;
			TryServiceBuyer();
		}

		private void TryServiceBuyer()
		{
			if(_buyersQueue.Count < 1)
			{
				return;
			}

			var buyer = _buyersQueue.Dequeue();
			buyer.FirstInQueue();
			buyer.RouteToBoxOffice();
		}

		private void ActionAreaOnStopActionHandler(Stack _)
		{
			_isReadyToServiceBuyer = false;
		}

		public void AddToQueue(Buyer buyer)
		{
			if(_buyersQueue.Count == 0 && _isReadyToServiceBuyer)
			{
				buyer.RouteToBoxOffice();
			}

			_buyersQueue.Enqueue(buyer);
		}

		public void ReceiveBoxWithProducts(Stack stack)
		{
			var boxProduct = MyGardenPool.Insance.Get(_boxProductsPrefab, _stack.Position, Quaternion.identity, Vector3.one);
			boxProduct.OnArrived += BoxProductsOnArrivedHandler;
			stack.Add(boxProduct);
		}

		private void BoxProductsOnArrivedHandler(IStackable stackable, Stack stack)
		{
			stackable.OnArrived -= BoxProductsOnArrivedHandler;
			OnTakeBox?.Invoke();
			TryServiceBuyer();
		}
	}
}