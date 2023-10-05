using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityUtils.AddressableUtils
{
	[Serializable]
	public class AddressableLoader
#if UNITY_EDITOR
		: ISerializationCallbackReceiver
#endif
	{
		public const string FieldName = nameof(prefabReference);

		[SerializeField] private AssetReference prefabReference;

		[field: SerializeField, HideInInspector]
		public string Name { get; private set; }

		public async Task<GameObject> Load(Transform parent)
		{
			if (prefabReference == null) return null;

			AsyncOperationHandle<GameObject> operation = prefabReference.InstantiateAsync(parent, false);

			return await operation.Task;
		}

		public async Task<Addressable<TComponent>> Load<TComponent>(Transform parent)
			where TComponent : Component
		{
			GameObject obj = await Load(parent);
			return new Addressable<TComponent>(obj, obj.GetComponent<TComponent>());
		}

#if UNITY_EDITOR
		public void OnBeforeSerialize()
		{
			try
			{
				Name = prefabReference.editorAsset.name;
			}
			catch (Exception) { }
		}
		public void OnAfterDeserialize() { }
#endif
	}

	[Serializable]
	public class LazyAddressable<T> : AddressableLoader, IDisposable
		where T : Component
	{
		public static implicit operator bool(LazyAddressable<T> lazyAddressable) => (bool)lazyAddressable.loaded;

		[SerializeField] private Transform parent;

		private Task<Addressable<T>> loadTask;
		private Addressable<T> loaded;

		public async Task<T> GetComponent()
		{
			loadTask ??= Load<T>(parent);

			if (!loaded)
				loaded = await loadTask;

			return loaded;
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
