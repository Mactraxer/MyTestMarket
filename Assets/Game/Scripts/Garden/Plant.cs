using Fruits;
using Pool;
using System.Collections;
using System.Linq;
using UnityEngine;
using Stack = Stacks.Stack;

namespace Plants
{
	public class Plant : MonoBehaviour
	{
		[SerializeField] private FruitSlot[] _fruitPoints;
		[SerializeField] private Fruit _prefab;
		[SerializeField] private float _growDelay;
		[SerializeField] private Stack _stack;

		private FruitData _fruitData;

		public void Setup(FruitData fruitData)
		{
			_fruitData = fruitData;
		}

		private void OnDestroy()
		{
			StopCoroutine(GrowLoop());
		}

		private IEnumerator GrowLoop()
		{
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
		}

		private void FruitOnRipeFruitHandler(Fruit fruit)
		{
			_stack.Add(fruit, true);
		}

		public void Active()
		{
			StartCoroutine(GrowLoop());
		}
	}
}