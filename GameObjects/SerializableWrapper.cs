using System;
using UnityEngine;

namespace WarmongersAPI.External.unity_utils.GameObjects
{
	[Serializable]
	public struct SerializedWrapper<T>
		where T : UnityEngine.Object
	{
		public static implicit operator T(SerializedWrapper<T> sw) => sw.Value;

		[field: SerializeField]
		public T Value { get; private set; }
	}
}
