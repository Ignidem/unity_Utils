using UnityEngine;
using Utilities.Conversions;

namespace UnityUtils.PropertyAttributes
{
	[System.Serializable]
	public class PolymorphicUnityObjectReference<T> : IConvertible<T>
		where T : Object
	{
		public static implicit operator T(PolymorphicUnityObjectReference<T> poly) => poly.reference;

		[SerializeField]
		private T reference;

		public T Convert()
		{
			return reference;
		}
	}
}
