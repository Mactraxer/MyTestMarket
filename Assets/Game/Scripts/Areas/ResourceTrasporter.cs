using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stack = Stacks.Stack;

namespace Resources
{
	public class ResourceTrasporter : MonoBehaviour
	{
		[SerializeField] private float _transferDelayBetweenProducts;

		private Queue<TransferTicket> _buyersTicketQueue = new();
		private Coroutine _buyerTransferProductsCoroutine;
		private Coroutine _playerTrasnferCoroutine;
		private WaitForSeconds _waitForDelay;

		private void Start()
		{
			_waitForDelay = new WaitForSeconds(_transferDelayBetweenProducts);
		}

		public void StartTransfer(Stack sourceStack, Stack destinationStack)
		{
			if(sourceStack is PlayerStack || destinationStack is PlayerStack)
			{
				if(_playerTrasnferCoroutine != default)
					StopCoroutine(_playerTrasnferCoroutine);
				_playerTrasnferCoroutine = StartCoroutine(TransferProducts(sourceStack, destinationStack));
			}
			else
			{
				_buyersTicketQueue.Enqueue(new TransferTicket(sourceStack, destinationStack));
				TryStartTransfer();
			}
		}

		public void StopTransfer(Stack source, Stack destination)
		{
			if(source is PlayerStack || destination is PlayerStack && _playerTrasnferCoroutine != default)
			{
				StopCoroutine(_playerTrasnferCoroutine);
			}
			else if(_buyerTransferProductsCoroutine != default)
			{
				StopCoroutine(_buyerTransferProductsCoroutine);
				_buyerTransferProductsCoroutine = default;

				if(_buyersTicketQueue.Count > 0)
				{
					TryStartTransfer();
				}
			}
		}

		private void TryStartTransfer()
		{
			if(_buyerTransferProductsCoroutine == default)
			{
				var ticket = _buyersTicketQueue.Dequeue();
				_buyerTransferProductsCoroutine = StartCoroutine(TransferProducts(ticket.SourceStack, ticket.DestinationStack));
			}
		}

		private IEnumerator TransferProducts(Stack sourceStack, Stack destinationStack)
		{
			while(true)
			{
				yield return _waitForDelay;
				if(sourceStack.Count > 0 && !destinationStack.Max)
				{
					sourceStack.Remove(destinationStack);
				}
			}
		}
	}
}