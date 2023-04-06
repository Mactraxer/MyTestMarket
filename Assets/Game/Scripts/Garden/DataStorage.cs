using Common;
using UnityEngine;

namespace Data
{
	public class DataStorage : MonoBehaviour
	{
		public static void SavePurchase(int id)
		{
			PlayerPrefs.SetInt(Constants.PURCHASE_KEY + id.ToString(), 1);
		}

		public static int GetMoney()
		{
			return PlayerPrefs.GetInt(Constants.MONEY_KEY, default);
		}

		public static void SaveMoney(int money)
		{
			PlayerPrefs.SetInt(Constants.MONEY_KEY, money);
		}

		internal static int GetPurchase(int id)
		{
			return PlayerPrefs.GetInt(Constants.PURCHASE_KEY + id.ToString(), default);
		}
	}
}