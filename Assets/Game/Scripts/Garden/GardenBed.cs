using Areas;
using Fruits;
using Plants;
using UnityEngine;

namespace Garden
{
	public class GardenBed : MonoBehaviour
	{
		[SerializeField] private BuyArea _buyArea;
		[SerializeField] private ActionArea _interactableArea;
		[SerializeField] private FruitData _fruitData;
		[SerializeField] private Plant _plant;

		private void Start()
		{
			_plant.Setup(_fruitData);
			_plant.Active();
		}
	}
}