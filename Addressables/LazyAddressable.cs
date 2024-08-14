using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Logger;
using UnityUtils.GameObjects;

namespace UnityUtils.AddressableUtils
{
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

			comp = await InstantiateObject(parent);
			return comp;
		}

		public TaskAwaiter<T> GetAwaiter() => GetComponent().GetAwaiter();

		public override void Dispose()
		{
			if (IsLoading)
			{
				//was not yet loaded
				DisposeAsync().LogException();
				return;
			}

			if (comp) comp.DestroySelfObject();
			base.Dispose();
		}
	}
}
