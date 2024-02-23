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
	}
}
