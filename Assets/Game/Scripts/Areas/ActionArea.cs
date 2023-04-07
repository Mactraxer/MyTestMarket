using Stacks;
using System;

public class ActionArea : InteractableArea
{
	public event Action<Stack> OnStartAction;
	public event Action<Stack> OnStopAction;

	protected override bool CheckInteractCondition()
	{
		return true;
	}

	protected override void Interact(Stack stack)
	{
		OnStartAction?.Invoke(stack);
	}

	protected override void StopInteract(Stack stack)
	{
		OnStopAction?.Invoke(stack);
	}
}