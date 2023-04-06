using System.Collections;
using UnityEngine;
using Stack = Stacks.Stack;

public abstract class InteractableArea : MonoBehaviour
{
	[SerializeField] private float _activationDelay;
	
	private Stack _interactStack;

	private Coroutine _coroutine;
	private WaitForSeconds _waitForDelay;

	private void Start()
	{
		_waitForDelay = new WaitForSeconds(_activationDelay);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(!other.gameObject.TryGetComponent(out Stack stack))
		{
			return;
		}

		_interactStack = stack;
		if(CheckInteractCondition())
		{
			_coroutine = StartCoroutine(ActivationCoroutine());
		}
	}

	private IEnumerator ActivationCoroutine()
	{
		yield return _waitForDelay;
		Intetacted(_interactStack);
	}

	private void OnTriggerExit(Collider other)
	{
		if(!other.gameObject.TryGetComponent(out Stack _))
		{
			return;
		}

		StopCoroutine(_coroutine);
		_interactStack = default;
	}

	protected abstract bool CheckInteractCondition();

	protected abstract void Intetacted(Stack stack);

	protected abstract void StopInteract();
}