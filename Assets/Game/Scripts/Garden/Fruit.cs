using DG.Tweening;
using Pool;
using Stacks;
using System;
using UnityEngine;

namespace Fruits
{
	[SelectionBase]
	public class Fruit : MonoBehaviour, IPoolable, IStackable
	{
		public event Action<Fruit> OnRipeFruit;
		public event Action<IStackable, Stack> OnArrived;
		public event Action<IPoolable> OnRelease;

		[SerializeField] private MeshFilter _meshFilter;
		[SerializeField] private MeshRenderer _meshRenderer;

		private FruitData _fruitData;
		private Sequence _growSequence;
		private bool _isRipe;
		private int _poolId;

		public bool IsRipe => _isRipe;

		public int PoolID => _poolId;

		public bool IsActive => gameObject.activeSelf;

		public Transform Transform => transform;

		public void Setup(FruitData fruitData)
		{
			_fruitData = fruitData;
			_isRipe = false;
			_meshFilter.mesh = fruitData.Mesh;
			_meshRenderer.material = fruitData.Material;
		}

		public void Grow()
		{
			_growSequence = DOTween.Sequence().OnComplete(() =>
			{
				_isRipe = true;
				OnRipeFruit?.Invoke(this);
			});
			_growSequence.Append(transform.DOScale(Vector3.one, _fruitData.GrowTime));
			_growSequence.Append(transform.DOScale(Vector3.one * 1.15f, 0.2f));
			_growSequence.Append(transform.DOScale(Vector3.one, 0.1f));
		}

		public void SetActive(bool active)
		{
			gameObject.SetActive(active);
		}

		public void FlyTo(Stack stack)
		{
			transform.DOLocalJump(stack.Position, 2, 1, 0.5f).OnComplete(() =>
			{
				OnArrived?.Invoke(this, stack);
			});
		}

		public void ChangeParent(Transform transform)
		{
			this.transform.SetParent(transform);
		}

		public void Dispose()
		{
			OnRelease?.Invoke(this);
		}

		public void SetId(int id)
		{
			_poolId = id;
		}
	}
}