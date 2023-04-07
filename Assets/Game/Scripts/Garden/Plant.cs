using Fruits;
using Pool;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Plants
{
	public class Plant : MonoBehaviour
	{
		[SerializeField] private FruitSlot[] _fruitPoints;
		[SerializeField] private Fruit _prefab;
		[SerializeField] private float _growDelay;
		[SerializeField] private GardenBedHeap _stack;

		private FruitData _fruitData;
		private Coroutine _growCoroutine;
		private bool _isGrow = false;

		public void Setup(FruitData fruitData)
		{
			_fruitData = fruitData;
			_stack.OnRemoveFruit += StackOnRemoveFruitHandler;
		}

		private void StackOnRemoveFruitHandler(Fruit fruit)
		{
			foreach(var slot in _fruitPoints)
			{
				if(slot.TryFreeSlot(fruit))
				{
					break;
				}
			}

			if(_stack.Count == 0 && !_isGrow)
			{
				_growCoroutine = StartCoroutine(GrowLoop());
			}
		}

		private void OnDestroy()
		{
			StopCoroutine(_growCoroutine);
		}

		private IEnumerator GrowLoop()
		{
			_isGrow = true;
			var waitForGrowDelay = new WaitForSeconds(_growDelay + _fruitData.GrowTime);
			while(_fruitPoints.Any(slot => !slot.IsBusy))
			{
				yield return waitForGrowDelay;
				var slot = _fruitPoints.First(slot => !slot.IsBusy);
				var fruit = MyGardenPool.Insance.Get(_prefab, slot.transform.position, Quaternion.identity, Vector3.zero);
				fruit.OnRipeFruit += FruitOnRipeFruitHandler;
				fruit.Setup(_fruitData);
				fruit.Grow();
				slot.SetFruit(fruit);
			}

			_isGrow = false;
		}

		private void FruitOnRipeFruitHandler(Fruit fruit)
		{
			_stack.Add(fruit, true);
		}

		public void Active()
		{
			_growCoroutine = StartCoroutine(GrowLoop());
		}
	}
}