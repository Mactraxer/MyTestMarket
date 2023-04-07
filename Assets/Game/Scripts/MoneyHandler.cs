using Data;
using System;

namespace Resources
{
	public class MoneyHandler
	{
		public static event Action<int> OnChangeAmount;

		public static MoneyHandler Instance
		{
			get
			{
				if(_instance == default)
				{
					_instance = new MoneyHandler();
					return _instance;
				}

				return _instance;
			}
		}

		private static MoneyHandler _instance;

		private int _money;

		private MoneyHandler()
		{
			_money = DataStorage.GetMoney();
			OnChangeAmount?.Invoke(_money);
		}

		public int GetCount()
		{
			return _money;
		}

		public void AddMoney(int amount)
		{
			if(amount < 0)
			{
				throw new InvalidOperationException("Can not add a negative amount of money");
			}

			ChangeAmount(amount);
		}

		public bool TrySpend(int amount)
		{
			if(amount > _money)
			{
				return false;
			}

			ChangeAmount(amount);
			return true;
		}

		private void ChangeAmount(int amount)
		{
			_money += amount;
			DataStorage.SaveMoney(_money);
			OnChangeAmount?.Invoke(_money);
		}
	}
}