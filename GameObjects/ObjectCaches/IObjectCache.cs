using System;
using System.Threading.Tasks;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public interface IObjectCache : IDisposable 
	{
		bool IsAlive { get; }
	}

	public interface IObjectCache<TKey, TValue> : IObjectCache
		where TValue : ICacheableObject
	{
		TValue this[TKey key] { get; }
		bool Peek(TKey key);
		TValue Pop(TKey key);
		void Cache(TKey key, TValue value);
		TValue Create(TKey key);
		Task<TValue> CreateAsync(TKey key)
		{
			return Task.FromResult(Create(key));
		}
	}
}
