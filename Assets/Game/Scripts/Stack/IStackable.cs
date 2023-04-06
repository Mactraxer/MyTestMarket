using System;
using UnityEngine;

namespace Stacks
{
	public interface IStackable
	{
		public event Action<IStackable, Stack> OnArrived;

		void ChangeParent(Transform transform);
		void FlyTo(Stack stack);
	}
}