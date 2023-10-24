using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityUtils.GameObjects;

namespace UnityUtils.AddressableUtils
{
	[System.Serializable]
	public class AddressableReference<T> : System.IDisposable
#if UNITY_EDITOR
		, ISerializationCallbackReceiver
#endif
		where T : Object
	{
		public const string FieldName = nameof(prefabReference);

		[SerializeField] private AssetReference prefabReference;

		[field: SerializeField, HideInInspector]
		public string Name { get; private set; }

		public bool IsLoaded => loadTask?.IsCompleted ?? false;
		public bool IsLoading => !(loadTask?.IsCompleted ?? true);

		private Task loadTask;
		private IAddressable<T> adrsLoad;

		private bool IsComponent => typeof(T).IsComponentType();

		public async Task<T> Instantiate()
		{
			IAddressable<T> adrs = await Load();
			return Object.Instantiate(adrs.Target);
		}

		public async Task<K> Instantiate<K>()
		{
			IAddressable<T> adrs = await Load();
			T instance = Object.Instantiate(adrs.Target);

			K Destroy()
			{
				//Object should be destroyed as its reference will be lost if nothing is returned;
				Object obj = instance;
				obj.SelfDestructObject();
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
			AsyncOperationHandle<K> result = Addressables.LoadAssetAsync<K>(prefabReference.RuntimeKey);
			loadTask = result.Task;
			return await result.Task;
		}

#if UNITY_EDITOR
		public void OnBeforeSerialize()
		{
			try
			{
				Object asset = prefabReference.editorAsset;

				if (!asset) return;

				Name = asset.name;
			}
			catch (System.Exception) { }
		}

		public void OnAfterDeserialize() { }
#endif
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
			if (comp) comp.SelfDestructObject();
			base.Dispose();
		}
	}
}
