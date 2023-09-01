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
		protected override LabelDrawType LabelType => LabelDrawType.Foldout;

		private void Init(SerializedProperty prop, PolymorphicAttribute polyAttr)
		{
			if (polyAttr.WasInitialized) return;

			int listIndex = prop.GetIndex();
			polyAttr.SetFieldInfo(prop.GetParent(), fieldInfo, listIndex);
		}

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;
			Init(property, polyAttr);

			int index = property.GetIndex();
			position = position.MoveY(SpacedLineHeight);
			PolyTypeDropdown(position, index, polyAttr);
			EditorGUI.indentLevel++;

			position = position.MoveY(SpacedLineHeight);
			DefaultGUI(ref position, property);

			EditorGUI.indentLevel--;
			EditorGUI.EndFoldoutHeaderGroup();
			return 0;
		}

		private Rect PolyTypeDropdown(Rect position, int listIndex, PolymorphicAttribute polyAttr)
		{
			Rect popupPos = position.SetHeight(LineHeight).MoveX(Spacing * 3);
			int index = EditorGUI.Popup(popupPos, "Type", polyAttr.Index, polyAttr.options);
			polyAttr.ChangeIndex(index, listIndex);

			return position;
		}
	}
}
