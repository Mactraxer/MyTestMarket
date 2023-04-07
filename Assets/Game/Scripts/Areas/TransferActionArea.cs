using Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stack = Stacks.Stack;

namespace Areas
{
	public class TransferActionArea : InteractableArea
	{
		[SerializeField] private float _actionDelay;
		[SerializeField] private bool _isReceiver;
		[SerializeField] private Stack _stack;

		private WaitForSeconds _waitForDelay;
		private Coroutine _playerTransferCoroutine;
		private Coroutine _transferCoroutine;
		private List<Stack> _interactList = new();
		private bool _isInteract;
		private ResourceTrasporter _resourceTrasporter;

		private void Start()
		{
			_waitForDelay = new WaitForSeconds(_actionDelay);
		}

		protected override bool CheckInteractCondition()
		{
			return true;
		}

		protected override void Interact(Stack stack)
		{
			if(stack is PlayerStack)
			{
				_playerTransferCoroutine = StartCoroutine(TransferItemsLoop(stack, _stack));
			}
			else
			{
				_interactList.Add(stack);
				if(_transferCoroutine == default)
				{
					TryRunNextInteract();
				}
			}
		}

		private IEnumerator TransferItemsLoop(Stack fromStack, Stack toStack)
		{
			while(true)
			{
				yield return _waitForDelay;
				if(fromStack.Count > 0)
				{
					fromStack.Remove(toStack);
				}
			}
		}

		protected override void StopInteract(Stack stack)
		{
			if(stack is PlayerStack)
			{
				StopCoroutine(_playerTransferCoroutine);
			}
			else
			{
				if(_transferCoroutine != default)
					StopCoroutine(_transferCoroutine);

				_transferCoroutine = default;
				TryRunNextInteract();
			}
		}

		private void TryRunNextInteract()
		{
			if(_interactList.Count > 0)
			{
				_transferCoroutine = StartCoroutine(TransferItemsLoop(_stack, _interactList[0]));
				_interactList.RemoveAt(0);
			}
		}
	}
}