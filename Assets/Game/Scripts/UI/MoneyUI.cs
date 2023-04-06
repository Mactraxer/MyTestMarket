using Resources;
using TMPro;
using UnityEngine;

namespace UI
{
	public class MoneyUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _moneyText;

		private void Awake()
		{
			MoneyHandler.OnChangeAmount += MoneyHandlerOnChangeAmountHandler;
		}

		private void MoneyHandlerOnChangeAmountHandler(int amount)
		{
			_moneyText.text = amount.ToString();
		}
	}
}