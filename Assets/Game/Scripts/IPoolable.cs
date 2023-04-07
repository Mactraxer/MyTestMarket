using System;
using UnityEngine;

namespace Pool
{
	public interface IPoolable
	{
		public event Action<IPoolable> OnRelease;
		public int ID { get; }
		public bool IsActive { get; }
		public Transform Transform { get; }
		public void SetActive(bool active);
		public void Dispose();
	}
}