using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UObject = UnityEngine.Object;

namespace UnityUtils.AddressableUtils
{
	public interface IAddressable<T> : IDisposable
	{
		UObject Asset { get; }
		T Target { get; }
		bool IsAlive { get; }
	}

	public class ObjectAddressable<TObject> : IAddressable<TObject>
	{
		public static implicit operator bool(ObjectAddressable<TObject> adrs)
		{
			return adrs != null && adrs.IsAlive;
		}

		public readonly TObject objectTarget;
		public UObject Asset => objectTarget as UObject;
		public TObject Target => objectTarget;

		public ObjectAddressable(TObject obj)
		{
			objectTarget = obj;
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
			if (objectTarget is UObject obj)
				UObject.Destroy(obj);
			Addressables.Release(objectTarget);
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

		public ComponentAddressable(GameObject obj, T t) : base(obj)
		{
			this.targetComponent = t;
			gameObject = obj;
		}
	}
}
