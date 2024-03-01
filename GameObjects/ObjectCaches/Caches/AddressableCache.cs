using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public class AddressableCache<TValue> : BaseObjectCache<string, TValue>
		where TValue : Object, ICacheableObject
	{
		private readonly Dictionary<string, AsyncOperationHandle<TValue>> handles = new();

		public AddressableCache(Transform parent) : base(parent) { }

		private AsyncOperationHandle<TValue> GetHandle(string key)
		{
			if (handles.TryGetValue(key, out AsyncOperationHandle<TValue> handle))
				return handle;

			return handles[key] = Addressables.LoadAssetAsync<TValue>(key);
		}

		public TValue Create(string key)
		{
			Debug.LogWarning("Async is recommended for addressable cache usages");
			AsyncOperationHandle<TValue> handle = GetHandle(key);
			TValue prefab = handle.WaitForCompletion();
			return Object.Instantiate(prefab);
		}

		public async Task<TValue> CreateAsync(string key)
		{
			AsyncOperationHandle<TValue> handle = GetHandle(key);
			TValue prefab = await handle.Task;
			return Object.Instantiate(prefab);
		}
	}
}
