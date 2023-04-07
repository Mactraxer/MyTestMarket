using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stack = Stacks.Stack;

public abstract class InteractableArea : MonoBehaviour
{
	[SerializeField] private float _activationDelay;

	private Coroutine _coroutine;
	private WaitForSeconds _waitForDelay;
	private Dictionary<Stack, Coroutine> _interactStacks = new();

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

		if(CheckInteractCondition())
		{
			_interactStacks.Add(stack, StartCoroutine(ActivationCoroutine(stack)));
		}
	}

	private IEnumerator ActivationCoroutine(Stack stack)
	{
		yield return _waitForDelay;
		Interact(stack);
	}

	private void OnTriggerExit(Collider other)
	{
		if(!other.gameObject.TryGetComponent(out Stack stack))
		{
			return;
		}

		StopInteract(stack);
		if (!_interactStacks.ContainsKey(stack))
		{
			return;
		}

		var interactCoroutine = _interactStacks[stack];
		_interactStacks.Remove(stack);
		StopCoroutine(interactCoroutine);
	}

	protected abstract bool CheckInteractCondition();

	protected abstract void Interact(Stack stack);

	protected abstract void StopInteract(Stack stack);
}