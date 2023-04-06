using Fruits;
using System;
using UnityEngine;

namespace Buyers
{
	[Serializable]
	public struct RequirementProduct
	{
		[SerializeField] private FruitData _fruitData;
		[SerializeField] private int _count;

		public FruitData FruitData => _fruitData;
		public int Count => _count;
	}
}