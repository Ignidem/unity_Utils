using System;
using System.Reflection;
using UnityEngine;

namespace UnityUtils.Serialization
{
	[System.Serializable]
	public struct TypeField<T> : ISerializationCallbackReceiver
	{
		[SerializeField]
		private string fullname;

		public Type Type
		{
			readonly get => type;
			set
			{
				type = value;
				fullname = type?.FullName;
			}
		}
		private Type type;

		public void OnAfterDeserialize()
		{
			if (!string.IsNullOrEmpty(fullname))
				type = Assembly.GetAssembly(typeof(T)).GetType(fullname);
		}

		public readonly void OnBeforeSerialize() { }
	}
}
