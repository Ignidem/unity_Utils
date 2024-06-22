using System;
using System.Threading.Tasks;

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
}
