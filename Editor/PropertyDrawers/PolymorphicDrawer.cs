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

			Type selectedType = polyAttr.SelectedType;

			position = DrawProperty(position, property, selectedType);

			EditorGUI.indentLevel--;
			EditorGUI.EndFoldoutHeaderGroup();
			return 0;
		}

		private Rect DrawProperty(Rect position, SerializedProperty property, Type selectedType)
		{
			if (selectedType == null) return position;

			if (!selectedType.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				DefaultGUI(ref position, property);
				return position;
			}
			
			position = position.MoveY(Spacing);

			object instance = fieldInfo.GetValue(property.GetParent());
			int index = property.GetIndex();

			UnityEngine.Object value = (instance is IList list ? list[index] : instance) as UnityEngine.Object;
			UnityEngine.Object newValue = EditorGUI.ObjectField(position, GUIContent.none, value, selectedType, true);

			if (newValue == value) return position;

			if (instance is IList nlist)
			{
				nlist[index] = newValue;
			}
			else
			{
				fieldInfo.SetValue(property.GetParent(), newValue); 
			}

			property.serializedObject.ApplyModifiedProperties();

			return position;
		}

		private Rect PolyTypeDropdown(Rect position, int listIndex, PolymorphicAttribute polyAttr)
		{
			Rect popupPos = position.SetHeight(LineHeight).MoveX(Spacing * 3);
			popupPos = popupPos.SetWidth(ViewWidth - popupPos.x);
			int index = EditorGUI.Popup(popupPos, "Type", polyAttr.Index, polyAttr.options);
			polyAttr.ChangeIndex(index, listIndex);

			return position;
		}
	}
}
