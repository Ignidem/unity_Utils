using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Utils.Delegates;

namespace UnityUtils.Editor.SerializedProperties
{
	public class SerializedArray
	{
		public static implicit operator SerializedArray(SerializedProperty prop)
		{
			return new SerializedArray(prop);
		}
		public static implicit operator SerializedProperty(SerializedArray array)
		{
			return array.property;
		}

		public readonly SerializedProperty property;

		public int Size
		{
			get => property.arraySize;
			set => property.arraySize = value;
		}

		public SerializedProperty this[int index]
		{
			get => property.GetArrayElementAtIndex(index);
		}

		public SerializedArray(SerializedProperty array)
		{
			if (!array.isArray)
				throw new Exception($"Property {array.propertyPath} is not an array.");

			this.property = array;
		}

		public SerializedProperty Add()
		{
			return Insert(Size);
		}
		public SerializedProperty Insert(int index)
		{
			property.InsertArrayElementAtIndex(index);
			return property.GetArrayElementAtIndex(index);
		}
		public void RemoveAt(int index)
		{
			property.DeleteArrayElementAtIndex(index);
		}
	}

	public class SerializedArray<T> : SerializedArray, IEnumerable<T>
	{
		public static implicit operator SerializedArray<T>(SerializedProperty prop)
		{
			return new SerializedArray<T>(prop);
		}
		public static implicit operator SerializedProperty(SerializedArray<T> array)
		{
			return array.property;
		}

		protected static readonly Dictionary<Type, IPropertyDelegate> propDelegates = new()
		{
			[typeof(string)] = new Property<SerializedProperty, string>
			(
				(prop) => prop.stringValue,
				(prop, value) => prop.stringValue = value
			),
		}; 
		
		new public T this[int index]
		{
			get
			{
				SerializedProperty element = base[index];
				return propDelegate.Get(element);
			}
			set
			{
				SerializedProperty element = base[index];
				propDelegate.Set(element, value);
			}
		}

		private readonly Property<SerializedProperty, T> propDelegate;
		public SerializedArray(SerializedProperty array) : base(array)
		{
			propDelegate = propDelegates.GetProperty<SerializedProperty, T>();
		}

		public void Copy(IList<T> list)
		{
			Size = list.Count;
			for (int i = 0; i < Size; i++)
			{
				this[i] = list[i];
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < Size; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
