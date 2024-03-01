using System.Threading.Tasks;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public static class ObjectCacheEx
	{
		public static bool TryPop<TKey, TValue>(this IObjectCache<TKey, TValue> cache, TKey key, out TValue value)
			where TValue : ICacheableObject
		{
			if (!cache.Peek(key))
			{
				value = default;
				return false;
			}

			value = cache.Pop(key);
			return true;
		}

		public static TValue PopOrCreate<TKey, TValue>(this ISyncObjectCache<TKey, TValue> cache, TKey key)
			where TValue : ICacheableObject
		{
			return cache.Peek(key) ? cache.Pop(key) : cache.Create(key);
		}

		public static Task<TValue> PopOrCreate<TKey, TValue>(this IAsyncObjectCache<TKey, TValue> cache, TKey key)
			where TValue : ICacheableObject
		{
			return cache.Peek(key) ? Task.FromResult(cache.Pop(key)) : cache.Create(key);
		}
	}
}
