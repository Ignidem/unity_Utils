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

		public bool IsLoaded => loadTask?.IsCompleted ?? false;
		public bool IsLoading => !(loadTask?.IsCompleted ?? true);

		private Task loadTask;
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
			return Object.Instantiate(adrs.Target, parent);
		}

		public async Task<K> InstantiateAs<K>(Transform parent = null)
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
}
