using System;
using System.Threading.Tasks;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public interface ISyncObjectCache<TKey, TValue> : IObjectCache<TKey, TValue>
		where TValue : ICacheableObject
	{
		TValue this[TKey key] { get; }
		TValue Create(TKey key);
	}

	public interface IAsyncObjectCache<TKey, TValue> : IObjectCache<TKey, TValue>
		where TValue : ICacheableObject
	{
		Task<TValue> this[TKey key] { get; }
		Task<TValue> Create(TKey key);
	}
}
