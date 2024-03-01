﻿using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityUtils.GameObjects;

namespace UnityUtils.AddressableUtils
{
	public interface IAddressableKey : System.IDisposable
	{
		string AssetGUID { get; }

		Task<IAddressable<T>> Load<T>();
	}

	[System.Serializable]
	public class AddressableReference<T> : IAddressableKey
		where T : Object
	{
		public const string FieldName = nameof(prefabReference);

		[SerializeField]
		private AssetReference prefabReference;

		[field: SerializeField, HideInInspector]
		public string Name { get; private set; }

		[field: SerializeField, HideInInspector]
		public string Path { get; private set; }

		public object RuntimeKey => prefabReference.RuntimeKey;
		public string AssetGUID => prefabReference.AssetGUID;

		public bool IsLoaded => loadTask?.IsCompleted ?? false;
		public bool IsLoading => !(loadTask?.IsCompleted ?? true);

		private Task loadTask;
		private IAddressable<T> adrsLoad;

		private bool IsComponent => typeof(T).IsComponentType();

		public AddressableReference() { }

		public AddressableReference(AssetReference asset, string name, string path)
		{
			prefabReference = asset;
			Name = name;
			Path = path;
		}

		public override bool Equals(object obj)
		{
			return obj is IAddressableKey key && AssetGUID == key.AssetGUID;
		}

		public override int GetHashCode()
		{
			return AssetGUID.GetHashCode();
		}

		public async Task<T> Instantiate(Transform parent = null)
		{
			IAddressable<T> adrs = await Load();
			return Object.Instantiate(adrs.Target, parent);
		}

		public async Task<K> Instantiate<K>(Transform parent = null)
		{
			IAddressable<T> adrs = await Load();
			T instance = Object.Instantiate(adrs.Target, parent);

			K Destroy()
			{
				//Object should be destroyed as its reference will be lost if nothing is returned;
				Object obj = instance;
				obj.DestroySelfObject();
				return default;
			}

			if (instance is K _k) return _k;

			System.Type kType = typeof(K);
			if (!kType.IsComponentType())
				return Destroy();

			if (instance is GameObject obj && obj.TryGetInSelfOrChildren(out K comp))
				return comp;

			return Destroy();
		}

		async Task<IAddressable<K>> IAddressableKey.Load<K>()
		{
			if (adrsLoad == null)
				await Load();

			if (adrsLoad is IAddressable<K> adrs)
				return adrs;

			if (typeof(K).IsComponentType())
			{
				GameObject obj = await LoadAs<GameObject>();
				obj.TryGetInSelfOrChildren(out K comp);
				return new ComponentAddressable<K>(obj, comp);
			}

			return new ObjectAddressable<K>(adrsLoad.Asset is K _k ? _k : default);
		}

		public async Task<IAddressable<T>> Load()
		{
			if (adrsLoad != null)
				return adrsLoad;

			if (!IsComponent)
			{
				T t = await LoadAs<T>();
				return adrsLoad = new ObjectAddressable<T>(t);
			}

			GameObject obj = await LoadAs<GameObject>();

			obj.TryGetInSelfOrChildren(out T comp);

			return adrsLoad = new ComponentAddressable<T>(obj, comp);
		}

		public virtual void Dispose()
		{
			if (IsLoading)
			{
				//was not yet loaded
				DisposeAsync();
				return;
			}

			adrsLoad?.Dispose();
		}

		private async void DisposeAsync()
		{
			await loadTask;
			Dispose();
		}

		private async Task<K> LoadAs<K>()
		{
			if (loadTask != null)
				return await (Task<K>)loadTask;

			AsyncOperationHandle<K> result = Addressables.LoadAssetAsync<K>(prefabReference.RuntimeKey);
			loadTask = result.Task;
			return await result.Task;
		}
	}

	[System.Serializable]
	public class LazyAddressable<T> : AddressableReference<T>
		where T : Component
	{
		public static implicit operator bool(LazyAddressable<T> lazyAddressable)
		{
			return lazyAddressable.comp;
		}

		[SerializeField] private Transform parent;

		private T comp;

		public async Task<T> GetComponent()
		{
			if (comp) return comp;

			comp = await Instantiate();
			comp.transform.SetParent(parent);
			return comp;
		}

		public TaskAwaiter<T> GetAwaiter() => GetComponent().GetAwaiter();

		public override void Dispose()
		{
			if (comp) comp.DestroySelfObject();
			base.Dispose();
		}
	}
}
