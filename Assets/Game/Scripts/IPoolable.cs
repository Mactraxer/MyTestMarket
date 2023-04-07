using System;
using UnityEngine;

namespace Pool
{
	public interface IPoolable
	{
		public event Action<IPoolable> OnRelease;
		public int PoolID { get; }
		public bool IsActive { get; }
		public Transform Transform { get; }
		public void SetActive(bool active);
		public void Dispose();
		public void SetId(int id);
	}
}