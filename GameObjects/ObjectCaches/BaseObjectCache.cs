using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public abstract class BaseObjectCache<TKey, TValue> : IObjectCache<TKey, TValue>
		where TValue : ICacheableObject
	{
		public bool IsAlive => storedTransform && !isDisposed;
		private bool isDisposed;
		private readonly Dictionary<TKey, TValue> active = new();
		private readonly Dictionary<TKey, TValue> stored = new();
		private readonly Transform storedTransform;


		public TValue this[TKey key] 
		{
			get
			{
				if (this.TryPop(key, out TValue value))
					return value;

				return Create(key);
			}
		}

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
			return stored.ContainsKey(key);
		}
		public TValue Pop(TKey key)
		{
			TValue value = stored[key];
			stored.Remove(key);
			return value;
		}
		public virtual void Dispose()
		{
			foreach (TValue value in stored.Values)
			{
				value.Destroy();
			}

			stored.Clear();

			foreach (TValue value in active.Values)
			{
				if (!value.IsActive)
					value.Destroy();
			}

			Debug.LogWarning($"Cache did not destroy {active.Count} active {typeof(TValue).Name}");

			active.Clear();
		}
		public virtual TValue Create(TKey key)
		{
			TValue value = default;
			value?.OnReleased(this);
			return value;
		}

		public void Cache(TKey key, TValue value)
		{
			if (value.IsActive)
			{
				Set(active, key, value);
				value?.OnReleased(this);
				return;
			}
			
			value.Transform.SetParent(storedTransform);
			Set(stored, key, value);
			value?.OnCached(this);
		}
		protected void Set(Dictionary<TKey, TValue> cache, TKey key, TValue value)
		{
			if (cache.TryGetValue(key, out TValue prev))
			{
				if (prev.Equals(value))
					return;

				prev.Destroy();
			}

			cache[key] = value;
		}
	}
}
