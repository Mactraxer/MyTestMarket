using DG.Tweening;
using Stacks;
using System;
using UnityEngine;

namespace BoxOffices
{
	public class BoxProducts : MonoBehaviour, IStackable
	{
		public event Action<IStackable, Stack> OnArrived;

		public void ChangeParent(Transform transform)
		{
			this.transform.SetParent(transform);
		}

		public void FlyTo(Stack stack)
		{
			transform.DOJump(stack.Position, 1, 1, 0.5f).OnComplete(() =>
			{
				OnArrived?.Invoke(this, stack);
			});
		}
	}
}