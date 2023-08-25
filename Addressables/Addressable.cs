using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnityUtils.AddressableUtils
{
	public class Addressable<T> : IDisposable
	{
		public static implicit operator T(Addressable<T> adrs)
		{
			return adrs.wasReleased ? default : adrs.target;
		}

		public readonly GameObject gameObject;
		public readonly T target;
		private bool wasReleased;

		public Addressable(GameObject obj, T t)
		{
			this.gameObject = obj;
			this.target = t;
		}

		public void Dispose()
		{
			Addressables.Release(gameObject);
			wasReleased = true;
		}
	}
}
