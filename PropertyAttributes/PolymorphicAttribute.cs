using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utilities.Reflection;

namespace UnityUtils.PropertyAttributes
{
	public class PolymorphicAttribute : PropertyAttribute
	{
		public readonly bool nullable;
		public string[] options { get; private set; }
		private Type baseType;
		public bool WasInitialized => types != null;
		private Type[] types;
		private ConstructorInfo[] constructors;
		public int Index
		{
			get => nullable ? _index + 1 : _index;
			set => _index = nullable ? value - 1 : value;
		}

		private int _index;

		private object parent;
		private FieldInfo field;

		/// <summary>
		/// Uses the field's type as base type.
		/// </summary>
		/// <param name="nullable">Can the instance be null.</param>
		public PolymorphicAttribute(bool nullable = false)
		{
			this.nullable = nullable;
		}

		/// <summary>
		/// Defined base type.
		/// </summary>
		/// <param name="baseType">The base type the polymorphic objects must extend.</param>
		/// <param name="nullable">Can the instance be null.</param>
		public PolymorphicAttribute(Type baseType, bool nullable = false) : this(nullable)
		{
			Init(baseType);
		}

		private void Init(Type baseType)
		{
			this.baseType = baseType;
			types = baseType.GetSubTypes();

			int k = 0;
			options = new string[types.Length + (nullable ? 1 : 0)];
			if (nullable)
				options[k++] = "Null Reference";
			for (int i = 0; i < types.Length; i++, k++)
				options[k] = types[i].Name;

			constructors = types.Select(t =>
				t.GetConstructor(new Type[] { baseType }) ?? t.GetConstructor(new Type[0])
			).ToArray();
		}

		public void SetFieldInfo(object parent, FieldInfo field, int listIndex)
		{
			this.parent = parent;
			this.field = field;

			if (types == null) //did not init;
			{
				Type baseType = field.FieldType;
				
				if (baseType.IsArray)
				{
					baseType = baseType.GetElementType();
				}
				else if (baseType.IsGenericType) //Generic collections
				{
					Type[] generics = baseType.GetGenericArguments();
					baseType = generics[0];
				}

				Init(baseType);
			}

			object currentValue = GetFieldValue(listIndex);

			_index = currentValue == null ? -1 : Array.IndexOf(types, currentValue.GetType());
		}

		public bool ChangeIndex(int typeIndex, int listIndex)
		{
			if (typeIndex == Index) return false;

			Index = typeIndex;

			if (_index == -1)
			{
				SetFieldValue(null, listIndex);
				return true;
			}

			ChangeType(types[_index], listIndex);
			return true;
		}

		private void ChangeType(Type type, int listIndex)
		{
			if (field == null) return;

			object currentValue = GetFieldValue(listIndex);

			//Type was not truly changed;
			if (currentValue?.GetType() == type) return;

			ConstructorInfo constructorInfo = constructors[_index];
			if (constructorInfo == null)
			{
				Debug.LogWarning(string.Format("{0} does not have an empty constructor or a constructor with a {1} parameter.", 
					type.Name, baseType.Name));
			}

			object value = constructorInfo.GetParameters().Length == 0 
				? constructorInfo.Invoke(new object[] { })
				: constructorInfo.Invoke(new object[] { currentValue });
			SetFieldValue(value, listIndex);
		}

		private object GetFieldValue(int listIndex)
		{
			object value = field.GetValue(parent);
			if (listIndex != -1 && value is IList list)
				return list[listIndex];
			return value;
		}

		private void SetFieldValue(object value, int listIndex)
		{
			if (listIndex == -1)
			{
				field.SetValue(parent, value);
				return;
			}

			if (field.GetValue(parent) is IList list)
				list[listIndex] = value;
		}
	}
}
