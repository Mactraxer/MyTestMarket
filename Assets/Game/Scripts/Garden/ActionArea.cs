using System.Collections;
using UnityEngine;
using Stack = Stacks.Stack;

namespace Areas
{
	public class ActionArea : InteractableArea
	{
		[SerializeField] private float _actionDelay;
		[SerializeField] private bool _isReceiver;
		[SerializeField] private Stack _stack;

		private WaitForSeconds _waitForDelay;
		private Coroutine _coroutine;

		private void Start()
		{
			_waitForDelay = new WaitForSeconds(_actionDelay);
		}

		protected override bool CheckInteractCondition()
		{
			return true;
		}

		protected override void Intetacted(Stack stack)
		{
			_coroutine = StartCoroutine(TransferItemsLoop(stack));
		}

		private IEnumerator TransferItemsLoop(Stack stack)
		{
			if(_isReceiver)
			{
				while(stack.Count > 0)
				{
					yield return _waitForDelay;
					stack.Remove(_stack);
				}
			}
			else
			{
				while(_stack.Count > 0)
				{
					yield return _waitForDelay;
					_stack.Remove(stack);
				}
			}
		}

		protected override void StopInteract()
		{
			StopCoroutine(_coroutine);
		}
	}
}