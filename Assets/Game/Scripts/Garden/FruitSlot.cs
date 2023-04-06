using System;
using UnityEngine;

namespace Fruits
{
	public class FruitSlot : MonoBehaviour
	{
		private Fruit _fruit;

		public bool IsBusy => _fruit != default;

		public void SetFruit(Fruit fruit)
		{
			_fruit = fruit;
		}

		public Fruit GetFruit()
		{
			if(_fruit == default)
			{
				throw new NullReferenceException("Can't get fruit from slot because fruit is null");
			}

			return _fruit;
		}
	}
}