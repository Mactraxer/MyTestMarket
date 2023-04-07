using Areas;
using Buyers;
using DG.Tweening;
using MyExtensions;
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
		private BoxProducts _boxProduct;
		private Sequence _boxScaleSequence;

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
			TryGetProductBox();
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
			TryGetProductBox();
			_buyersQueue.Enqueue(buyer);
			if(_buyersQueue.Count == 1 && _isReadyToServiceBuyer)
			{
				TryServiceBuyer();
				return;
			}
		}

		private void TryGetProductBox()
		{
			if(_boxProduct == default)
			{
				_boxProduct = MyGardenPool.Insance.Get(_boxProductsPrefab, _stack.Position, Quaternion.identity, Vector3.zero, _stack.Transform, false);
				_boxScaleSequence = DOTween.Sequence();
				_boxScaleSequence.Append(_boxProduct.transform.DOScale(Vector3.one, 0.4f));
				_boxScaleSequence.Append(_boxProduct.transform.DOScale(Vector3.one * 1.15f, 0.2f));
				_boxScaleSequence.Append(_boxProduct.transform.DOScale(Vector3.one, 0.1f));
				_boxProduct.OnArrived += BoxProductsOnArrivedHandler;
			}
		}

		public void ReceiveBoxWithProducts(Stack stack)
		{
			_stack.Clear();
			stack.Add(_boxProduct);
		}

		private void BoxProductsOnArrivedHandler(IStackable stackable, Stack stack)
		{
			_boxProduct.OnArrived -= BoxProductsOnArrivedHandler;
			_boxProduct = default;
			OnTakeBox?.Invoke();
			this.ActionWithDelay(0.7f, () =>
			{
				TryServiceBuyer();
			});
		}
	}
}