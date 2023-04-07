using Pool;
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

		public Vector3 Position => _stackables.Count * _offset + _transform.localPosition;

		public int Count => _stackables.Count;

		public void Add(IStackable stackable, bool warp = false)
		{
			if (warp)
			{
				_stackables.Add(stackable);
				OnAdded(stackable);
				stackable.ChangeParent(_transform);
				OnChangeCount?.Invoke(Count);
				return;
			}

			stackable.OnArrived += OnArrivedStackHandler;
			stackable.ChangeParent(_transform);
			stackable.FlyTo(this);
		}

		protected virtual void OnAdded(IStackable stackable) { }
		protected virtual void OnRemoved(IStackable stackable) { }

		private void OnArrivedStackHandler(IStackable stackable, Stack stack)
		{
			_stackables.Add(stackable);
			OnAdded(stackable);
			OnChangeCount?.Invoke(Count);
			stackable.OnArrived -= OnArrivedStackHandler;
		}

		public void Remove(Stack stack)
		{
			if(_stackables.Count < 1)
				return;

			var stackable = _stackables[^1];
			_stackables.Remove(stackable);
			OnRemoved(stackable);
			stackable.OnArrived += OnLeaveStackHandler;
			stackable.ChangeParent(stack.transform);
			stackable.FlyTo(stack);
		}

		private void OnLeaveStackHandler(IStackable stackable, Stack stack)
		{
			OnChangeCount?.Invoke(Count);
			stack.Add(stackable, true);
			stackable.OnArrived -= OnLeaveStackHandler;
		}

		public void DisposeAll()
		{
			foreach(var stackable in _stackables)
			{
				if(stackable is not IPoolable poolable)
					continue;

				poolable.Dispose();
			}
		}
	}
}