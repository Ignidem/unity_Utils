using System;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public interface IObjectCache : IDisposable
	{
		bool IsAlive { get; }
	}

	public interface IObjectCache<TKey, TValue> : IObjectCache
		where TValue : ICacheableObject
	{
		void Cache(TKey key, TValue value);
		bool Peek(TKey key);
		TValue Pop(TKey key);
	}
}