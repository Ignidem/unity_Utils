using System;
using Utilities.Reflection;
using UnityEditor;
using UnityEngine;
using UnityUtils.Editor;
using UnityUtils.Editor.SerializedProperties;
using System.Linq;
using Utils.Collections;

namespace UnityUtils.Serialization.TypePopup.Editor
{
	[CustomPropertyDrawer(typeof(TypeField<>), true)]
	public class TypeFieldDrawer : ExtendedPropertyDrawer
	{
		private Type[] types;
		private string[] names;

		protected override LabelDrawType LabelType => LabelDrawType.None;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			if (types == null)
			{
				Type baseType = GetType(property);
				types = baseType.GetSubTypes();
				names = types.Select(t => t.Name).ToArray();
			}

			SerializedProperty assembly = property.GetRelativeProperty("assembly");
			SerializedProperty fullname = property.GetRelativeProperty("fullname");
			SerializedProperty name = property.GetRelativeProperty("name");

			int index = GetSelectedIndex(name);

			int nIndex = EditorGUI.Popup(position, property.displayName, index, names);
			if (nIndex != index)
			{
				Type type = nIndex == -1 ? null : types[nIndex];

				if (assembly != null)
					assembly.stringValue = type?.Assembly.FullName;

				if (fullname != null)
					fullname.stringValue = type?.FullName;

				if (name != null)
					name.stringValue = type?.Name;
			}

			return 0;
		}

		private static Type GetType(SerializedProperty property)
		{
			Type fieldType = property.GetFieldType(property.isArray);
			Type targetType = typeof(TypeField<>);
			if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == targetType)
				return fieldType.GetGenericArguments()[0];

			Type baseType = fieldType.GetBase(targetType);
			return baseType.GetGenericArguments()[0];
		}

		private int GetSelectedIndex(SerializedProperty name)
		{
			if (name == null)
				return -1;

			string nameValue = name.stringValue;
			if (string.IsNullOrEmpty(nameValue))
				return -1;

			int _d = nameValue.LastIndexOf('.');
			if (_d != -1)
				nameValue = nameValue[_d..];

			return names.IndexOf(nameValue);
		}
	}
}
