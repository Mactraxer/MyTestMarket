using DG.Tweening;
using Pool;
using Stacks;
using System;
using UnityEngine;

namespace BoxOffices
{
	public class BoxProducts : MonoBehaviour, IStackable, IPoolable
	{
		private int _poolId;

		public int PoolID => _poolId;

		public bool IsActive => gameObject.activeSelf;

		public Transform Transform => transform;

		public event Action<IStackable, Stack> OnArrived;
		public event Action<IPoolable> OnRelease;

		public void ChangeParent(Transform transform)
		{
			this.transform.SetParent(transform);
		}

		public void Dispose()
		{
			OnRelease?.Invoke(this);
		}

		public void FlyTo(Stack stack)
		{
			transform.DOLocalJump(stack.Position, 1, 1, 0.5f).OnComplete(() =>
			{
				OnArrived?.Invoke(this, stack);
			});
		}

		public void SetActive(bool active)
		{
			gameObject.SetActive(active);
		}

		public void SetId(int id)
		{
			_poolId = id;
		}
	}
}