using System.Collections.Generic;
using UnityEngine;
using Utils.Collections;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public abstract class BaseObjectCache<TKey, TValue> : IObjectCache<TKey, TValue>
		where TValue : ICacheableObject
	{
		public bool IsAlive => storedTransform && !isDisposed;
		private bool isDisposed;
		private readonly Dictionary<TKey, List<TValue>> active = new();
		private readonly Dictionary<TKey, List<TValue>> stored = new();
		private readonly Transform storedTransform;

		public BaseObjectCache(Transform parent = null, bool withController = false)
		{
			GameObject go = new GameObject(typeof(TValue).Name + " Cache");
			storedTransform = go.transform;
			storedTransform.SetParent(parent);

			if (withController)
			{
				ObjectCacheController controller = ObjectCacheController.GetOrCreate(parent);
				controller.SetCache(this);
			}
		}

		public BaseObjectCache()
		{

		}

		public bool Peek(TKey key)
		{
			return stored.ContainsKey(key) && stored[key].Count > 0;
		}
		public TValue Pop(TKey key)
		{
			TValue value = stored[key].Pop();
			return value;
		}
		public virtual void Dispose()
		{
			DestroyAll(stored);
			stored.Clear();

			DestroyAll(active);
			active.Clear();
		}

		private void DestroyAll(Dictionary<TKey, List<TValue>> dict)
		{
			foreach (List<TValue> list in dict.Values)
			{
				foreach (TValue value in list)
				{
					if (value.IsActive)
					{
						value.Release();
					}
					else
					{
						value.Destroy();
					}
				}
			}
		}

		public void Cache(TKey key, TValue value)
		{
			if (value.IsActive)
			{
				Set(active, key, value);
				value?.OnPop(this);
				return;
			}
			
			value.Transform.SetParent(storedTransform);
			Set(stored, key, value);
			value?.OnCached(this);
		}
		protected void Set(Dictionary<TKey, List<TValue>> cache, TKey key, TValue value)
		{
			if (!cache.TryGetValue(key, out List<TValue> prev))
				cache[key] = prev = new List<TValue>();

			prev.Add(value);
		}
	}
}
