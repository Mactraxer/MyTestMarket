using Data;
using Resources;
using Stacks;
using UnityEngine;

namespace Areas
{
	public class BuyArea : InteractableArea
	{
		[SerializeField] private int _price;
		[SerializeField] private GameObject _purchaseGameObject;

		private void Start()
		{
			if(DataStorage.GetPurchase(GetInstanceID()) != 0)
			{
				_purchaseGameObject.SetActive(true);
				gameObject.SetActive(false);
			}
		}

		protected override bool CheckInteractCondition()
		{
			return MoneyHandler.Instance.GetCount() > _price;
		}

		protected override void Interact(Stack stack)
		{
			DataStorage.SavePurchase(GetInstanceID());
			_purchaseGameObject.SetActive(true);
			gameObject.SetActive(false);
		}

		protected override void StopInteract(Stack stack)
		{
		}
	}
}