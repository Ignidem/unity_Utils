using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityUtils.AddressableUtils
{
	[Serializable]
	public class AddressableLoader
	{
		public const string FieldName = nameof(prefabReference);

		[SerializeField] private AssetReference prefabReference;

		public async Task<Addressable<TComponent>> Load<TComponent>(Transform parent)
			where TComponent : Component
		{
			AsyncOperationHandle<GameObject> operation = prefabReference.InstantiateAsync(parent);
			GameObject obj = await operation;
			if (parent)	obj.transform.SetParent(parent, true);
			return new Addressable<TComponent>(obj, obj.GetComponent<TComponent>());
		}
	}
}
