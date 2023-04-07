using Stacks;
using UI;
using UnityEngine;

namespace Player
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private Stack _stack;
		[SerializeField] private PlayerUI _playerUI;
		[SerializeField] private PlayerAnimation _playerAnimation;

		private void Start ()
		{
			_stack.OnChangeCount += StackOnChangeCountHandler;
		}

		private void StackOnChangeCountHandler(int count)
		{
			if(_stack.Max)
			{
				_playerUI.ShowMaxCapacityText();
			}
			else
			{
				_playerUI.HideMaxCapacityText();
			}

			if(_stack.Count > 0)
			{
				_playerAnimation.PickUp();
			}
			else
			{
				_playerAnimation.PickDown();
			}
		}
	}
}