using BoxOffices;
using Buyers;
using Pool;
using Resources;
using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField] private Transform _shelvingPoint;
	[SerializeField] private Transform _boxOfficePoint;
	[SerializeField] private Transform _exitPoint;
	[SerializeField] private Transform _startPoint;
	[SerializeField] private BoxOffice _boxOffice;
	[SerializeField] private Buyer _buyerPrefab;

	private void Start()
	{
		MoneyHandler.Instance.AddMoney(10);
		var buyer = MyGardenPool.Insance.Get(_buyerPrefab, _startPoint.position, Quaternion.identity, Vector3.one);
		buyer.Setup(_shelvingPoint, _boxOfficePoint, _exitPoint, _boxOffice);
		buyer.Active();
	}
}