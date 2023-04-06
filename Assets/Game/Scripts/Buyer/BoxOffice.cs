using Buyers;
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

		private BoxProducts _boxProducts;
		private Queue<Buyer> _buyersQueue = new();

		public Stack Stack => _stack;

		public void AddToQueue(Buyer buyer)
		{
			if(_buyersQueue.Count == 0)
			{
				buyer.ReadyToPay();
			}

			_buyersQueue.Enqueue(buyer);
		}

		public void ReceiveBoxWithProducts(Stack stack)
		{
			_boxProducts.OnArrived += BoxProductsOnArrivedHandler;
			stack.Add(_boxProducts);
		}

		private void BoxProductsOnArrivedHandler(IStackable stackable, Stack stack)
		{
			stackable.OnArrived -= BoxProductsOnArrivedHandler;
			OnTakeBox?.Invoke();
		}
	}
}