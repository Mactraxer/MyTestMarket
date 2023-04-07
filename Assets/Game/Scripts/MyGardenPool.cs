using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pool
{
	public class MyGardenPool : MonoBehaviour
	{
		[SerializeField] private List<IPoolable> _poolables = new();

		private static MyGardenPool _instance;

		public static MyGardenPool Insance
		{
			get
			{
				if(_instance != default)
				{
					return _instance;
				}

				PoolGameObject = new GameObject("MyGardenPool");
				_instance = PoolGameObject.AddComponent<MyGardenPool>();
				return _instance;
			}
		}

		public static GameObject PoolGameObject { get; private set; }

		public T Get<T>(T prefab, Vector3 position, Quaternion identity, Vector3 scale) where T : Object, IPoolable
		{
			if(_poolables.Any(item => !item.IsActive))
			{
				var poolItem = (T)_poolables.First(item => !item.IsActive);
				poolItem.Transform.position = position;
				poolItem.Transform.rotation = identity;
				poolItem.Transform.localScale = scale;
				poolItem.SetActive(true);
				return poolItem;
			}
			else
			{
				var poolItem = Instantiate(prefab, position, identity, transform);
				poolItem.OnRelease += PoolItemOnReleaseHandler;
				poolItem.Transform.localScale = scale;
				_poolables.Append(poolItem);
				poolItem.SetActive(true);
				return poolItem;
			}
		}

		private void PoolItemOnReleaseHandler(IPoolable poolable)
		{
			poolable.SetActive(false);
			poolable.Transform.SetParent(transform);
			poolable.Transform.position = transform.position;
		}
	}
}