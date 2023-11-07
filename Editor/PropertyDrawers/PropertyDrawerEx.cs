﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityUtils.Editor.SerializedProperties;
using UnityUtils.RectUtils;

namespace UnityUtils.Editor
{
	public enum LabelDrawType
	{
		None,
		Label,
		Foldout,
		HeaderFoldout,
	}

	public abstract class ExtendedPropertyDrawer : PropertyDrawer
	{
		protected const float IndentWidth = 38;
		protected static float IndentX => IndentWidth * EditorGUI.indentLevel;
		protected static float Spacing => EditorGUIUtility.standardVerticalSpacing * 2;
		protected static float LineHeight => EditorGUIUtility.singleLineHeight;
		protected static float SpacedLineHeight => LineHeight + EditorGUIUtility.standardVerticalSpacing;
		protected static float FieldWidth => EditorGUIUtility.fieldWidth;
		public static float ViewWidth => EditorGUIUtility.currentViewWidth;

		protected virtual LabelDrawType LabelType => LabelDrawType.HeaderFoldout;
		private readonly List<bool> folded = new();

		private readonly Dictionary<string, float> heights = new();

		protected abstract float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int index = Math.Max(0, property.GetIndex());
			while (folded.Count <= index)
			{
				folded.Add(false);
			}

			EditorGUI.BeginProperty(position, label, property);

			Rect start = position;
			position = position.SetHeight(LineHeight);

			switch (LabelType)
			{
				case LabelDrawType.Label:
					position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
					break;
				case LabelDrawType.Foldout:
					folded[index] = !EditorGUI.Foldout(position, !folded[index], label);
					break;
				case LabelDrawType.HeaderFoldout:
					folded[index] = !EditorGUI.BeginFoldoutHeaderGroup(position, !folded[index], label);
					break;
			}

			float extraHeight = folded[index] ? 0 : DrawProperty(ref position, property, label);

			EditorGUI.EndProperty();

			if (LabelType == LabelDrawType.HeaderFoldout)
			{
				EditorGUI.EndFoldoutHeaderGroup();
			}

			float height = (position.y - start.y) + LineHeight + extraHeight;

			heights[property.propertyPath] = height;
		}

		protected void DefaultGUI(ref Rect position, SerializedProperty property)
		{
			SerializedProperty prop = property.Copy();
			string path = property.propertyPath;
			position = position.MoveY(Spacing);

			while (prop.Next(prop.propertyType == SerializedPropertyType.ManagedReference))
			{
				//Makes sure we don't step out while drawing an element from a list;
				if (!prop.propertyPath.StartsWith(path)) continue;

				float pHeight = EditorGUI.GetPropertyHeight(prop);
				position = position.SetHeight(pHeight);
				EditorGUI.PropertyField(position, prop, true);
				position = position.MoveY(pHeight + Spacing);
			}
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
