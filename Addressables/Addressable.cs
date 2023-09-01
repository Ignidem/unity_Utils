using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnityUtils.AddressableUtils
{
	public class Addressable : IDisposable
	{
		public static implicit operator bool(Addressable adrs)
		{
			return adrs != null && adrs.gameObject && !adrs.WasReleased;
		}

		public readonly GameObject gameObject;

		public Addressable(GameObject obj)
		{
			this.gameObject = obj;
		}

		protected bool WasReleased { get; private set; }

		public void Dispose()
		{
			UnityEngine.Object.Destroy(gameObject);
			Addressables.Release(gameObject);
			WasReleased = true;
		}
	}

	public class Addressable<T> : Addressable
	{
		public static implicit operator bool(Addressable<T> adrs)
		{
			return adrs != null && adrs.gameObject && !adrs.WasReleased && adrs.target != null;
		}

		public static implicit operator T(Addressable<T> adrs)
		{
			return !adrs ? default : adrs.target;
		}

		public readonly T target;

		public Addressable(GameObject obj, T t) : base(obj)
		{
			this.target = t;
		}
	}
}
