using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityUtils.AddressablesUtils
{
	public class AddressablesCache<TValue> : AddressablesCache<string, TValue>
		where TValue : UnityEngine.Object
	{
		public override AsyncOperationHandle<TValue> Load(string key)
		{
			if (!handlers.TryGetValue(key, out AsyncOperationHandle<TValue> handler))
			{
				AssetReference assetRef = new AssetReference(key);
				handler = assetRef.LoadAssetAsync<TValue>();
				handlers[key] = handler;
			}

			return handler;
		}
	}

	public class AddressablesCache<TKey, TValue> : IDisposable
		where TValue : UnityEngine.Object
	{
		public readonly Dictionary<TKey, AsyncOperationHandle<TValue>> handlers = new();

		public virtual AsyncOperationHandle<TValue> Load(TKey key)
		{
			if (!handlers.TryGetValue(key, out AsyncOperationHandle<TValue> handler))
			{
				handler = Addressables.LoadAssetAsync<TValue>(key);
				handlers[key] = handler;
			}

			return handler;
		}

		public Task<TValue> LoadValue(TKey key)
		{
			AsyncOperationHandle<TValue> handler = Load(key);
			return handler.Task;
		}

		public async Task<TValue> Instantiate(TKey key, Transform parent)
		{
			TValue value = await LoadValue(key);
			return UnityEngine.Object.Instantiate(value, parent);
		}
		
		public virtual void Dispose()
		{
			foreach (AsyncOperationHandle<TValue> value in handlers.Values)
			{
				Addressables.Release(value);
			}
		}
	}
}
