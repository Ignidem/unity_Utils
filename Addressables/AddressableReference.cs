using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityUtils.GameObjects;
using Utils.Logger;

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

		public System.Type Type => typeof(T);

		public bool IsValid => prefabReference != null && !string.IsNullOrEmpty(AssetGUID);

		[SerializeField]
		private AssetReference prefabReference;

		[field: SerializeField, HideInInspector]
		public string Name { get; private set; }

		[field: SerializeField, HideInInspector]
		public string Path { get; private set; }

		public object RuntimeKey => prefabReference.RuntimeKey;
		public string AssetGUID => prefabReference.AssetGUID;

		public bool IsLoaded => LoadTask?.IsCompleted ?? false;
		public bool IsLoading => !(LoadTask?.IsCompleted ?? true);

		protected Task LoadTask { get; private set; }
		private object handle;
		private IAddressable<T> adrsLoad;

		private bool IsComponent => typeof(T).IsComponentType();

		public AddressableReference() { }

		public AddressableReference(AssetReference asset, string name = null, string path = null)
		{
			prefabReference = asset;
			Name = name ?? asset.SubObjectName;
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

		public async Task<T> InstantiateObject(Transform parent = null)
		{
			IAddressable<T> adrs = await Load();
			if (parent != null)
			{
				return Object.Instantiate(adrs.Target, parent, false);
			}
			else
			{
				return Object.Instantiate(adrs.Target);
			}
		}

		async Task<IAddressable<K>> IAddressableKey.Load<K>()
		{
			if (adrsLoad == null)
				await Load();

			if (adrsLoad is IAddressable<K> adrs)
				return adrs;

			if (typeof(K).IsComponentType())
			{
				AsyncOperationHandle<GameObject> objHandle = LoadHandleAs<GameObject>();
				GameObject obj = await objHandle;
				obj.TryGetInSelfOrChildren(out K comp);
				return new ComponentAddressable<K>(obj, comp, objHandle);
			}

			AsyncOperationHandle<K> handle = adrsLoad.Handle is AsyncOperationHandle<K> _handle ? _handle : default;
			return new ObjectAddressable<K>(adrsLoad.Asset is K _k ? _k : default, handle);
		}

		public async Task<IAddressable<T>> Load()
		{
			if (adrsLoad != null)
				return adrsLoad;

			if (!IsComponent)
			{
				AsyncOperationHandle<T> handle = LoadHandleAs<T>();
				T t = await handle;
				return adrsLoad = new ObjectAddressable<T>(t, handle);
			}

			AsyncOperationHandle<GameObject> objHandle = LoadHandleAs<GameObject>();
			GameObject obj = await objHandle;
			obj.TryGetInSelfOrChildren(out T comp);

			return adrsLoad = new ComponentAddressable<T>(obj, comp, objHandle);
		}

		public virtual void Dispose()
		{
			if (IsLoading)
			{
				//was not yet loaded
				DisposeAsync().LogException();
				return;
			}

			adrsLoad?.Dispose();
			adrsLoad = null;
			LoadTask = null;
			handle = null;
		}

		public virtual async Task DisposeAsync()
		{
			if (LoadTask != null)
				await LoadTask;
			Dispose();
		}

		private AsyncOperationHandle<K> LoadHandleAs<K>()
		{
			if (handle is not AsyncOperationHandle<K> result)
			{
				result = Addressables.LoadAssetAsync<K>(prefabReference.RuntimeKey);
				handle = result;
			}

			LoadTask ??= result.Task;
			return result;
		}
	}
}
