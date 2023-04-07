using Areas;
using Resources;
using Stacks;
using UnityEngine;

namespace Shelvings
{
	public class Shelving : MonoBehaviour
	{
		[SerializeField] private ActionArea _actionArea;
		[SerializeField] private ResourceTrasporter _resourceTrasporter;
		[SerializeField] private Stack _stack;

		private void Start () 
		{
			_actionArea.OnStartAction += ActionAreaOnStartActionHandler;
			_actionArea.OnStopAction += ActionAreaOnStopActionHandler;
		}

		private void ActionAreaOnStopActionHandler(Stack stack)
		{
			if(stack is PlayerStack)
			{
				_resourceTrasporter.StopTransfer(stack, _stack);
			}
			else
			{
				_resourceTrasporter.StopTransfer(_stack, stack);
			}
		}

		private void ActionAreaOnStartActionHandler(Stack stack)
		{
			if(stack is PlayerStack)
			{
				_resourceTrasporter.StartTransfer(stack, _stack);
			}
			else
			{
				_resourceTrasporter.StartTransfer(_stack, stack);
			}
		}
	}
}