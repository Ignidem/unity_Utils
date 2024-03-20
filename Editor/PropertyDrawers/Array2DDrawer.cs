using UnityUtils.Editor.SerializedProperties;
using UnityUtils.RectUtils;
using UnityEditor;
using UnityEngine;
using UnityUtils.Arrays;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace UnityUtils.Editor
{
	[CustomPropertyDrawer(typeof(Array2D<>))]
	public class Array2DDrawer : ExtendedPropertyDrawer
	{
		private const string ElementsField = Array2D<object>.ElementsFieldName;
		protected override LabelDrawType LabelType => LabelDrawType.None;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			Vector2Int size = DrawSize(ref position, property);
			if (size.x != 0 && size.y != 0)
				DrawGrid(ref position, property, size);

			property.serializedObject.ApplyModifiedProperties();
			return SpacedLineHeight * 3;
		}

		private Vector2Int DrawSize(ref Rect position, SerializedProperty property)
		{
			Vector2Int size = GetSize(property);
			position = position.SetHeight(LineHeight);
			Vector2Int newSize = EditorGUI.Vector2IntField(position, GUIContent.none, size);
			if (size == newSize)
				return size;

			return Resize(property, newSize);
		}

		private Vector2Int GetSize(SerializedProperty prop)
		{
			SerializedProperty list = prop.FindPropertyRelative(ElementsField);
			int x = list.arraySize;
			int y = x <= 0 ? 0 : list.GetArrayElementAtIndex(0).FindPropertyRelative(ElementsField).arraySize;
			return new Vector2Int(x, y);
		}

		private Vector2Int Resize(SerializedProperty prop, Vector2Int size)
		{
			SerializedProperty list = prop.FindPropertyRelative(ElementsField);
			if (list.arraySize != size.x)
				list.arraySize = size.x;

			for (int i = 0; i < size.x; i++)
			{
				SerializedProperty subArrayObj = list.GetArrayElementAtIndex(i);
				SerializedProperty subList = subArrayObj.FindPropertyRelative(ElementsField);
				subList.arraySize = size.y;
			}

			return size;
		}

		protected virtual void DrawGrid(ref Rect position, SerializedProperty prop, Vector2Int size)
		{
			SerializedProperty list = prop.FindPropertyRelative(ElementsField);

			float width = Mathf.Max(10, (ViewWidth - IndentWidth) / size.x);
			Rect origin = Indent(position.MoveY(LineHeight))
				.SetWidth(width).SetHeight(LineHeight);

			for (int x = 0; x < size.x; x++)
			{
				if (x >= list.arraySize) break;

				SerializedProperty subArrayObj = list.GetArrayElementAtIndex(x);
				SerializedProperty subList = subArrayObj.FindPropertyRelative(ElementsField);
				if (subList == null) continue;

				for (int y = 0; y < size.y; y++)
				{
					if (y >= subList.arraySize) break;

					SerializedProperty element = subList.GetArrayElementAtIndex(y);
					if (element == null) continue;

					position = origin.Move(new Vector2(x * width, y * LineHeight));
					DrawCell(position, element);
				}
			}
		}

		protected virtual void DrawCell(Rect pos, SerializedProperty element)
		{
			EditorGUI.PropertyField(pos, element, GUIContent.none);
		}
	}
}
