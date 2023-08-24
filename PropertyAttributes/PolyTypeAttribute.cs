using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utilities.Reflection;

namespace UnityUtils.PropertyAttributes
{
	public class PolyTypeAttribute : PropertyAttribute
	{
		private readonly Type[] types;
		private readonly ConstructorInfo[] constructors;
		public string[] options;
		public int Index
		{
			get => _index;
			set
			{
				if (_index == value) return;

				_index = value;
				ChangeType(types[_index]);
			}
		}
		private int _index;

		private object parent;
		private FieldInfo field;

		public PolyTypeAttribute(Type baseType) 
		{
			types = baseType.GetSubTypes();
			options = types.Select(t => t.Name).ToArray();
			constructors = types.Select(t => t.GetConstructor(new Type[] { baseType })).ToArray();
		}

		public void SetFieldInfo(object parent, FieldInfo field)
		{
			this.parent = parent;
			this.field = field;

			object currentValue = GetFieldValue();
			Type currentType = currentValue.GetType();
			_index = Array.IndexOf(types, currentType);
		}

		private void ChangeType(Type type)
		{
			if (field == null) return;

			object currentValue = GetFieldValue();

			//Type was not truly changed;
			if (currentValue.GetType() == type) return;

			object value = constructors[_index].Invoke(new object[] { currentValue });
			SetFieldValue(value);
		}

		private object GetFieldValue()
		{
			return field.GetValue(parent);
		}

		private void SetFieldValue(object value)
		{
			field.SetValue(parent, value);
		}
	}
}
