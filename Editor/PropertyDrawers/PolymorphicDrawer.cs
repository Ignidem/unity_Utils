using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Event e = Event.current;

			Rect ddRect = GetDropdownRect(position);
			if (e.type == EventType.MouseDown && e.button == 1 && ddRect.Contains(e.mousePosition))
				TypeContextMenu();

			base.OnGUI(position, property, label);
		}

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;
			Init(property, polyAttr);

			int index = property.GetIndex();
			position = PolyTypeDropdown(position, index, polyAttr);
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
			Rect popupPos = GetDropdownRect(position);
			int index = EditorGUI.Popup(popupPos, "Type", polyAttr.Index, polyAttr.options);
			polyAttr.ChangeIndex(index, listIndex);
			return popupPos;
		}

		private static Rect GetDropdownRect(Rect position)
		{
			Rect popupPos = position.MoveY(SpacedLineHeight).SetHeight(LineHeight).MoveX(Spacing * 3);
			return popupPos.SetWidth(ViewWidth - popupPos.x);
		}

		private void TypeContextMenu()
		{
			PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;
			Type type = polyAttr.SelectedType;
			if (type == null || !TryGetScript(type, out _))
				return;

			GenericMenu context = new GenericMenu();
			context.AddItem(new GUIContent("Edit Script"), false, OpenClass);
			context.ShowAsContext();
		}

		private void OpenClass()
		{
			PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;
			Type type = polyAttr.SelectedType;
			if (type == null || !TryGetScript(type, out MonoScript script))
				return;

			AssetDatabase.OpenAsset(script);
		}

		private static bool TryGetScript(Type type, out MonoScript script)
		{
			string[] guids = AssetDatabase.FindAssets("t:script " + type.Name, new[] { "Assets" });

			static MonoScript GuidToScript(string guid)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);
				return AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
			}

			return script = guids.Select(GuidToScript).FirstOrDefault(script => script.GetClass() == type);
		}
	}
}
