using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stacks
{
	public abstract class Stack : MonoBehaviour
	{
		public event Action<int> OnChangeCount;

		[SerializeField] private Vector3 _offset;
		[SerializeField] private Transform _transform;

		private List<IStackable> _stackables = new();

		public Vector3 Position => _stackables.Count * _offset + _transform.position;

		public int Count => _stackables.Count;

		public void Add(IStackable stackable, bool warp = false)
		{
			if (warp)
			{
				_stackables.Add(stackable);
				stackable.ChangeParent(_transform);
				OnChangeCount?.Invoke(Count);
				return;
			}

			stackable.OnArrived += OnArrivedStackHandler;
			stackable.FlyTo(this);
		}

		private void OnArrivedStackHandler(IStackable stackable, Stack stack)
		{
			_stackables.Add(stackable);
			OnChangeCount?.Invoke(Count);
			stackable.ChangeParent(_transform);
			stackable.OnArrived -= OnArrivedStackHandler;
		}

		public void Remove(Stack stack)
		{
			if(_stackables.Count < 1)
				return;

			var stackable = _stackables[^1];
			_stackables.Remove(stackable);
			stackable.OnArrived += OnLeaveStackHandler;
			stackable.FlyTo(stack);
		}

		private void OnLeaveStackHandler(IStackable stackable, Stack stack)
		{
			OnChangeCount?.Invoke(Count);
			stack.Add(stackable, true);
			stackable.OnArrived -= OnLeaveStackHandler;
		}
	}
}