using Areas;
using Fruits;
using Plants;
using Resources;
using Stacks;
using UnityEngine;

namespace Garden
{
	public class GardenBed : MonoBehaviour
	{
		[SerializeField] private BuyArea _buyArea;
		[SerializeField] private ActionArea _actionArea;
		[SerializeField] private FruitData _fruitData;
		[SerializeField] private ResourceTrasporter _resourceTrasporter;
		[SerializeField] private Plant _plant;
		[SerializeField] private Stack _stack;

		private void Start()
		{
			_plant.Setup(_fruitData);
			_plant.Active();

			_actionArea.OnStartAction += ActionAreaOnStartActionHandler;
			_actionArea.OnStopAction += ActionAreaOnStopActionHandler;
		}

		private void ActionAreaOnStopActionHandler(Stack stack)
		{
			if(stack is not PlayerStack)
			{
				return;
			}

			_resourceTrasporter.StopTransfer(_stack, stack);
		}

		private void ActionAreaOnStartActionHandler(Stack stack)
		{
			if(stack is not PlayerStack)
			{
				return;
			}

			_resourceTrasporter.StartTransfer(_stack, stack);
		}
	}
}