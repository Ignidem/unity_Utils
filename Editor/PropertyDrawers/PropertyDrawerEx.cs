using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityUtils.RectUtils;

namespace UnityUtils.Editor
{
	public abstract class ExtendedPropertyDrawer : PropertyDrawer
	{
		protected const float IndentWidth = 38;
		protected static float IndentX => IndentWidth * EditorGUI.indentLevel;
		protected static float Spacing => EditorGUIUtility.standardVerticalSpacing * 2;
		protected static float LineHeight => EditorGUIUtility.singleLineHeight;
		protected static float SpacedLineHeight => LineHeight + EditorGUIUtility.standardVerticalSpacing;
		protected static float FieldWidth => EditorGUIUtility.fieldWidth;
		public static float ViewWidth => EditorGUIUtility.currentViewWidth;

		protected virtual bool DrawPrefixLabel => true;

		private readonly Dictionary<string, float> heights = new();

		private bool didInit;
		
		protected virtual void OnFirstGUI(SerializedProperty property) { }

		protected abstract float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!didInit)
			{
				OnFirstGUI(property);
				didInit = true;
			}

			EditorGUI.BeginProperty(position, label, property);

			Rect start = position;

			if (DrawPrefixLabel)
				position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			float extraHeight = DrawProperty(ref position, property, label);

			EditorGUI.EndProperty();

			float height = (position.y - start.y) + LineHeight + extraHeight;

			heights[property.propertyPath] = height;
		}

		protected float DefaultGUI(ref Rect position, SerializedProperty property)
		{
			float height = 0;
			SerializedProperty prop = property.Copy();
			while (prop.Next(prop.propertyType != SerializedPropertyType.String))
			{
				float pHeight = EditorGUI.GetPropertyHeight(prop);
				position = position.SetHeight(pHeight);
				EditorGUI.PropertyField(position, prop, true);
				height += pHeight;
				position = position.MoveY(pHeight);
			}

			return height;
		}

		protected float DefaultPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (heights.TryGetValue(property.propertyPath, out float height))
				return height;

			return DefaultPropertyHeight(property, label);
		}
	}
}
