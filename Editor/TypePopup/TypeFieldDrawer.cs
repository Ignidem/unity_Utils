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
	[CustomPropertyDrawer(typeof(TypeField<>))]
	public class TypeFieldDrawer : ExtendedPropertyDrawer
	{
		Type[] types;
		string[] names;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			Type type = property.GetFieldType(property.isArray).GenericTypeArguments[0];

			if (types == null)
			{
				types = type.GetSubTypes();
				names = types.Select(t => t.Name).ToArray();
			}

			const string fullname = nameof(fullname);
			SerializedProperty field = property.FindPropertyRelative(fullname);
			int index = -1;
			string name = field.stringValue;
			if (!string.IsNullOrEmpty(name))
			{
				int _d = name.LastIndexOf('.');
				if (_d != -1)
					name = name[_d..];

				index = names.IndexOf(field.stringValue);
			}

			int nIndex = EditorGUI.Popup(position, index, names);
			if (nIndex != index)
			{
				field.stringValue = nIndex == -1 ? null : types[nIndex].FullName;
			}

			return 0;
		}
	}
}
