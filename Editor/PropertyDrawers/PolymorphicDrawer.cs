using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityUtils.Editor.SerializedProperties;
using UnityUtils.PropertyAttributes;
using UnityUtils.RectUtils;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(PolymorphicAttribute))]
	public class PolymorphicDrawer : ExtendedPropertyDrawer
	{
		protected override void OnFirstGUI(SerializedProperty prop)
		{
			if (attribute is PolymorphicAttribute poly)
			{
				int index = prop.GetIndex();
				poly.SetFieldInfo(prop.GetParent(), fieldInfo, index);
			}
		}

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			int index = property.GetIndex();
			PolyTypeDropdown(position, index);
			position = position.MoveY(SpacedLineHeight);
			return DefaultGUI(ref position, property);
		}

		private Rect PolyTypeDropdown(Rect position, int listIndex)
		{
			if (attribute is not PolymorphicAttribute poly) return position;

			int index = EditorGUI.Popup(position.SetHeight(LineHeight), poly.Index, poly.options);
			poly.ChangeIndex(index, listIndex);

			return position;
		}
	}
}
