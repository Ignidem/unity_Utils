using System;

namespace UnityUtils.Serialization.Properties
{
	[Serializable]
	public struct Optional<T>
	{
		public static implicit operator bool(Optional<T> optional) => optional.enabled;
		public static implicit operator T(Optional<T> optional) => optional ? optional.value : default;
		
		public static T operator |(Optional<T> a, T b)
		{
			return a.enabled ? a.value : b;
		}
		public static T operator |(Optional<T> a, Optional<T> b)
		{
			return a.enabled ? a.value : (b.enabled ? b.value : default);
		}
		
		public bool enabled;
		public T value;
	}
}