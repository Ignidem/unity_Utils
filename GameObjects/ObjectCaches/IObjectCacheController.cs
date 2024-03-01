using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public interface IObjectCacheController
	{
		bool TryGetCache<TKey, TValue>(out IObjectCache<TKey, TValue> cache)
			where TValue : ICacheableObject;

		IObjectCache<TKey, TValue> GetOrCreateCache<TKey, TValue>(Func<IObjectCache<TKey, TValue>> constructor)
			where TValue : ICacheableObject;

		void SetCache<TKey, TValue>(IObjectCache<TKey, TValue> cache)
			where TValue : ICacheableObject;
	}

	public class ObjectCacheController : MonoBehaviour, IObjectCacheController
	{
		public static ObjectCacheController GlobalCache
		{
			get => _globalCache ? _globalCache : CreateCache(nameof(GlobalCache));
		}
		public static ObjectCacheController _globalCache;
		public static ObjectCacheController GetOrCreate(Transform parent)
		{
			if (parent == null)
				return GlobalCache;

			if (!parent.gameObject.TryGetComponent(out ObjectCacheController cache))
			{
				cache = CreateCache(nameof(ObjectCacheController)); 
			}

			return cache;
		}
		private static ObjectCacheController CreateCache(string name)
		{
			GameObject go = new GameObject(name);
			return go.AddComponent<ObjectCacheController>();
		}

		private readonly Dictionary<Type, IObjectCache> caches = new();

		private void OnDestroy()
		{
			foreach (var cache in caches)
				cache.Value.Dispose();
		}

		public bool TryGetCache<TKey, TValue>(out IObjectCache<TKey, TValue> cache)
			where TValue : ICacheableObject
		{
			Type key = typeof(TValue);
			if (caches.TryGetValue(key, out IObjectCache _cache) && _cache is IObjectCache<TKey, TValue> objectCache) 
			{
				cache = objectCache;
				return true;
			}

			cache = default;
			return false;
		}

		public IObjectCache<TKey, TValue> GetOrCreateCache<TKey, TValue>(Func<IObjectCache<TKey, TValue>> constructor)
			where TValue : ICacheableObject
		{
			Type key = typeof(TValue);
			if (caches.TryGetValue(key, out IObjectCache _cache))
			{
				if (_cache is not IObjectCache<TKey, TValue> objectCache)
				{
					throw new Exception("Cache already exists with a different key type");
				}
				
				return objectCache;
			}

			IObjectCache<TKey, TValue> _objectCache = constructor();
			caches[key] = _objectCache;
			return _objectCache;
		}

		public void SetCache<TKey, TValue>(IObjectCache<TKey, TValue> cache)
			where TValue : ICacheableObject
		{
			Type key = typeof(TValue);
			if (caches.ContainsKey(key))
			{
				throw new Exception("Cache already set");
			}

			caches[key] = cache;
		}
	}
}
