using BoxOffices;
using Buyers;
using Pool;
using Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField] private Transform _shelvingPoint;
	[SerializeField] private Transform _boxOfficePoint;
	[SerializeField] private Transform _exitPoint;
	[SerializeField] private Transform _startPoint;
	[SerializeField] private BoxOffice _boxOffice;
	[SerializeField] private Buyer _buyerPrefab;
	[SerializeField] private float _minBoundForRespawnBuyer;
	[SerializeField] private float _maxBoundForRespawnBuyer;
	[SerializeField] private int _maxBuyers;

	private List<Buyer> _buyers = new();

	private void Start()
	{
		MoneyHandler.Instance.AddMoney(10);
		StartCoroutine(RespawnBuyers());
	}

	private IEnumerator RespawnBuyers()
	{
		while(true)
		{
			var waitForDelay = new WaitForSeconds(Random.Range(_minBoundForRespawnBuyer, _maxBoundForRespawnBuyer));
			yield return waitForDelay;
			if(_buyers.Count < _maxBuyers)
			{
				var buyer = MyGardenPool.Insance.Get(_buyerPrefab, _startPoint.position, Quaternion.identity, Vector3.one, transform);
				buyer.OnServiced += BuyerOnServicedHandler;
				buyer.Setup(_shelvingPoint, _boxOfficePoint, _exitPoint, _boxOffice);
				buyer.Active();
				_buyers.Add(buyer);
			}
		}
	}

	private void BuyerOnServicedHandler(Buyer buyer)
	{
		buyer.OnServiced -= BuyerOnServicedHandler;
		_buyers.Remove(buyer);
		MoneyHandler.Instance.AddMoney(10);
	}
}