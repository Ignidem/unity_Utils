using System;
using System.Reflection;
using UnityEngine;

namespace UnityUtils.Serialization
{
	[Serializable]
	public class TypeField<T> : ITypeField, ISerializationCallbackReceiver
	{
		public static implicit operator Type(TypeField<T> field) => field.Type;

		[SerializeField] private string assembly;
		[SerializeField] private string fullname;
		[SerializeField] private string name;

		public Type BaseType => typeof(T);

		public Type Type
		{
			get => type;
			set
			{
				type = value;
				assembly = type?.Assembly.FullName;
				fullname = type?.FullName;
				name = type?.Name;
			}
		}
		private Type type;

		public void OnAfterDeserialize()
		{
			if (!string.IsNullOrEmpty(fullname))
				type = Assembly.Load(assembly).GetType(fullname);
		}

		public void OnBeforeSerialize() { }
	}
}
