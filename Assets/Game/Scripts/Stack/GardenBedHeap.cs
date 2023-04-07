using Fruits;
using Stacks;
using System;

public class GardenBedHeap : Stack
{
	public event Action<Fruit> OnRemoveFruit;

	protected override void OnRemoved(IStackable stackable)
	{
		base.OnRemoved(stackable);

		if (stackable is Fruit fruit)
		{
			OnRemoveFruit?.Invoke(fruit);
		}
	}
}