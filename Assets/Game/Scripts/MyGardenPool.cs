using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pool
{
	public class MyGardenPool : MonoBehaviour
	{
		private HashSet<IPoolable> _poolables = new();
		private Dictionary<int, Queue<IPoolable>> _poolItems = new();

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

		public T Get<T>(T prefab, Vector3 position, Quaternion identity, Vector3 scale, Transform parent, bool inWorldCoordinate = true) where T : Object, IPoolable
		{
			var queue = GetQueue(prefab.GetInstanceID());
			T poolItem;
			if (queue.Count > 0)
			{
				poolItem = (T) queue.Dequeue();
				poolItem.Transform.SetParent(parent);
				if(inWorldCoordinate)
				{
					poolItem.Transform.position = position;
					poolItem.Transform.rotation = identity;
				}
				else
				{
					poolItem.Transform.localPosition = position;
					poolItem.Transform.localRotation = identity;
				}

				poolItem.Transform.localScale = scale;
				poolItem.SetActive(true);
				_poolables.Add(poolItem);
				return poolItem;
			}

			poolItem = Instantiate(prefab, parent);
			poolItem.SetId(prefab.GetInstanceID());
			if(inWorldCoordinate)
			{
				poolItem.Transform.position = position;
				poolItem.Transform.rotation = identity;
			}
			else
			{
				poolItem.Transform.localPosition = position;
				poolItem.Transform.localRotation = identity;
			}

			poolItem.OnRelease += PoolItemOnReleaseHandler;
			poolItem.Transform.localScale = scale;
			_poolables.Add(poolItem);
			poolItem.SetActive(true);
			return poolItem;
/*
			if(_poolables.Any(item => !item.IsActive && item is T))
			{
				var poolItem = (T)_poolables.First(item => !item.IsActive && item is T);
				poolItem.Transform.SetParent(parent);
				if(inWorldCoordinate)
				{
					poolItem.Transform.position = position;
					poolItem.Transform.rotation = identity;
				}
				else
				{
					poolItem.Transform.localPosition = position;
					poolItem.Transform.localRotation = identity;
				}

				poolItem.Transform.localScale = scale;
				poolItem.SetActive(true);
				return poolItem;
			}
			else
			{
				var poolItem = Instantiate(prefab, parent);
				if(inWorldCoordinate)
				{
					poolItem.Transform.position = position;
					poolItem.Transform.rotation = identity;
				}
				else
				{
					poolItem.Transform.localPosition = position;
					poolItem.Transform.localRotation = identity;
				}
				
				poolItem.OnRelease += PoolItemOnReleaseHandler;
				poolItem.Transform.localScale = scale;
				_poolables.Add(poolItem);
				poolItem.SetActive(true);
				return poolItem;
			}*/
		}

		private Queue<IPoolable> GetQueue(int id)
		{
			if(_poolItems.TryGetValue(id, out var queue))
			{
				return queue;
			}

			queue = new Queue<IPoolable>();
			_poolItems.Add(id, queue);

			return queue;
		}

		private void PoolItemOnReleaseHandler(IPoolable poolable)
		{
			var queue = GetQueue(poolable.PoolID);
			if(!queue.Contains(poolable))
				queue.Enqueue(poolable);
			_poolables.Remove(poolable);

			poolable.SetActive(false);
			poolable.Transform.SetParent(transform);
			poolable.Transform.position = transform.position;
		}
	}
}