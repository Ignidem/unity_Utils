using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UObject = UnityEngine.Object;

namespace UnityUtils.AddressableUtils
{
	public interface IAddressable<T> : IDisposable
	{
		UObject Asset { get; }
		T Target { get; }
		AsyncOperationHandle<T> Handle { get; }
		bool IsAlive { get; }
	}

	public class ObjectAddressable<TObject> : IAddressable<TObject>
	{
		public static implicit operator bool(ObjectAddressable<TObject> adrs)
		{
			return adrs != null && adrs.IsAlive;
		}

		public readonly TObject objectTarget;
		public AsyncOperationHandle<TObject> Handle { get; }

		public UObject Asset => objectTarget as UObject;
		public TObject Target => objectTarget;

		public ObjectAddressable(TObject obj, AsyncOperationHandle<TObject> handle)
		{
			objectTarget = obj;
			this.Handle = handle;
		}

		protected bool WasReleased { get; private set; }

		public virtual bool IsAlive
		{
			get
			{
				return !WasReleased && objectTarget != null && Asset;
			}
		}

		public void Dispose()
		{
			if (Handle.IsValid())
			{
				Addressables.Release(Handle);
			}
			else
			{
				Addressables.Release(objectTarget); 
				Debug.LogError("Using Invalid Handle " + Asset);
			}

			WasReleased = true;
		}
	}

	public class ComponentAddressable<T> : ObjectAddressable<GameObject>, IAddressable<T>
	{
		public static implicit operator bool(ComponentAddressable<T> adrs)
		{
			return adrs != null && adrs.IsAlive;
		}

		public static implicit operator T(ComponentAddressable<T> adrs)
		{
			return !adrs ? default : adrs.targetComponent;
		}

		public readonly GameObject gameObject;
		public readonly T targetComponent;

		T IAddressable<T>.Target => targetComponent;

		public override bool IsAlive 
		{
			get
			{
				return base.IsAlive && targetComponent != null &&
					(targetComponent is UObject obj ? obj : true);
			}
		}

		AsyncOperationHandle<T> IAddressable<T>.Handle
		{
			get
			{
				return Handle is AsyncOperationHandle<T> _handle ? _handle : default;
			}
		}

		public ComponentAddressable(GameObject obj, T t, AsyncOperationHandle<GameObject> objHandle) 
			: base(obj, objHandle)
		{
			this.targetComponent = t;
			gameObject = obj;
		}
	}
}
