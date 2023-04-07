using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _maxCapacityText;

		private bool _isShow = false;

		private void Start()
		{
			_maxCapacityText.transform.DOScale(0, 0f);
		}

		public void ShowMaxCapacityText()
		{
			if(_isShow)
				return;
			_isShow = true;
			_maxCapacityText.transform.DOScale(1, 0.3f).SetEase(Ease.OutBounce);
		}

		public void HideMaxCapacityText()
		{
			if(!_isShow)
				return;
			_isShow = false;
			_maxCapacityText.transform.DOScale(0, 0.3f).SetEase(Ease.InBounce);
		}
	}
}