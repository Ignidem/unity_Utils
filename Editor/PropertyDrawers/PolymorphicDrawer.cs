using System;
using System.Collections;
using System.Linq;
using UnityEditor;
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

		public PolymorphicDrawer() : base()
		{
			EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
		}

		~PolymorphicDrawer()
		{
			EditorApplication.contextualPropertyMenu -= OnPropertyContextMenu;
		}

		private void Init(SerializedProperty prop, PolymorphicAttribute polyAttr)
		{
			if (polyAttr.WasInitialized) return;

			int listIndex = prop.GetIndex();
			polyAttr.SetFieldInfo(prop.GetParent(), fieldInfo, listIndex);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;
			Init(property, polyAttr);
			base.OnGUI(position, property, label);
		}

		private void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			if (property.propertyType != SerializedPropertyType.ManagedReference || property.boxedValue == null)
				return;

			PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;
			Type selectedType = polyAttr.SelectedType;
			if (selectedType == null || property.boxedValue.GetType() != selectedType)
				return;

			TypeContextMenu(menu);
		}

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;

			float width = CalcLabelSize(property.displayName).x;
			PolyTypeDropdown(position.MoveX(width), polyAttr, property);
			EditorGUI.indentLevel++;

			position = position.MoveY(SpacedLineHeight);

			Type selectedType = polyAttr.SelectedType;

			position = DrawProperty(position, property, selectedType);

			EditorGUI.indentLevel--;
			EditorGUI.EndFoldoutHeaderGroup();
			return -LineHeight;
		}

		protected override bool DrawLabel(SerializedProperty property, GUIContent label, 
			ref Rect position, int index, bool folded)
		{
			Rect pos = position;
			if (property.IsArrayElement() && folded)
			{
				PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;
				label = new GUIContent(polyAttr.GetFieldValue(index).GetType().Name);
				position = position.SetWidth(CalcLabelSize(label.text).x);
			}

			bool fold = base.DrawLabel(property, label, ref position, index, folded);
			position = pos;
			return fold;
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

		private Rect PolyTypeDropdown(Rect position, PolymorphicAttribute polyAttr, SerializedProperty property)
		{
			int listIndex = property.GetIndex();
			position = GetDropdownRect(position);
			string option = polyAttr.Index == -1 ? "Null Reference" : polyAttr.options[polyAttr.Index];
			float width = CalcLabelSize(option).x;
			Rect popupPos = position.SetWidth(width + 25);
			int index = EditorGUI.Popup(popupPos, polyAttr.Index, polyAttr.options);
			if (index == polyAttr.Index) return position;
			
			polyAttr.SetFieldInfo(property.GetParent(), fieldInfo, listIndex);
			if (polyAttr.ChangeIndex(index, listIndex, false, out object value))
			{
				property.boxedValue = value;
			}

			return position;
		}

		private static Rect GetDropdownRect(Rect position)
		{
			Rect popupPos = position.MoveX(Spacing * 3);
			return popupPos.SetWidth(ViewWidth - popupPos.x);
		}

		private void TypeContextMenu(GenericMenu context)
		{
			PolymorphicAttribute polyAttr = attribute as PolymorphicAttribute;
			Type type = polyAttr.SelectedType;
			if (type == null || !TryGetScript(type, out MonoScript script))
				return;

			context.AddItem(new GUIContent("Edit " + script.name), false, OpenClass);
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
