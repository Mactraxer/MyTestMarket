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
		[SerializeField] private int _capacity;

		[SerializeField] private List<IStackable> _stackables = new();

		public Vector3 Position => _stackables.Count * _offset;

		public int Count => _stackables.Count;

		public bool Max => Count == _capacity;

		public Transform Transform => _transform;

		public void Add(IStackable stackable, bool warp = false)
		{
			if(_stackables.Count == _capacity)
			{
				return;
			}

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
			_stackables.Add(stackable);
			stackable.FlyTo(this);
		}

		protected virtual void OnAdded(IStackable stackable) { }
		protected virtual void OnRemoved(IStackable stackable) { }

		private void OnArrivedStackHandler(IStackable stackable, Stack stack)
		{
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
			stack.Add(stackable, true);
			OnRemoved(stackable);
			stackable.OnArrived += OnLeaveStackHandler;
			stackable.ChangeParent(stack.Transform);
			stackable.FlyTo(stack);
		}

		private void OnLeaveStackHandler(IStackable stackable, Stack stack)
		{
			OnChangeCount?.Invoke(Count);
			
			stackable.OnArrived -= OnLeaveStackHandler;
		}

		public void Setup(int count)
		{
			_capacity = count;
		}

		public void Clear()
		{
			foreach(var stackable in _stackables)
			{
				if(stackable is IPoolable poolable)
				{
					poolable.Dispose();
				}
			} 

			_stackables.Clear();
		}
	}
}