using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityUtils.AddressableUtils
{
	[System.Serializable]
	public class AddressableReference<T>
#if UNITY_EDITOR
		: ISerializationCallbackReceiver
#endif
	{
		public const string FieldName = nameof(prefabReference);

		[SerializeField] private AssetReference prefabReference;

		[field: SerializeField]
		public string Name { get; private set; }

		private bool IsComponent
		{
			get
			{
				System.Type t = typeof(T);

				return t.IsAssignableFrom(typeof(Component)) || !t.IsAssignableFrom(typeof(Object));
			}
		}

		public async Task<IAddressable<T>> Load(Transform parent)
		{
			if (!IsComponent)
			{
				T t = await LoadAs<T>();
				return new ObjectAddressable<T>(t);
			}

			GameObject obj = await LoadAs<GameObject>();

			if (!obj.TryGetComponent(out T comp))
				comp = obj.GetComponentInChildren<T>();

			return new ComponentAddressable<T>(obj, comp);
		}

		private async Task<K> LoadAs<K>()
		{
			AsyncOperationHandle<K> result = Addressables.LoadAssetAsync<K>(prefabReference.RuntimeKey);
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
	public class LazyAddressable<T> : AddressableReference<T>, System.IDisposable
		where T : Component
	{
		public static implicit operator bool(LazyAddressable<T> lazyAddressable)
		{
			return lazyAddressable.loaded != null && lazyAddressable.loaded.IsAlive;
		}

		[SerializeField] private Transform parent;

		private Task<IAddressable<T>> loadTask;
		private IAddressable<T> loaded;

		public async Task<T> GetComponent()
		{
			loadTask ??= Load(parent);

			if (!loaded.IsAlive)
				loaded = await loadTask;

			return loaded.Target;
		}

		public TaskAwaiter<T> GetAwaiter() => GetComponent().GetAwaiter();

		public void Dispose()
		{
			if (loaded == null && !loadTask.IsCompleted)
			{
				//was not yet loaded
				DisposeAsync();
				return;
			}

			loaded?.Dispose();
		}

		private async void DisposeAsync()
		{
			await loadTask;
			Dispose();
		}
	}
}
