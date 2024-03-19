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
			IArray2D target = property.GetInstance() as IArray2D;
			DrawSize(ref position, target);

			Vector2Int size = target.Size;
			if (size.x != 0 && size.y != 0)
				DrawGrid(ref position, property, size);

			property.serializedObject.ApplyModifiedProperties();
			return SpacedLineHeight * 3;
		}

		private void DrawSize(ref Rect position, IArray2D array)
		{
			position = position.SetHeight(LineHeight);
			Vector2Int newSize = EditorGUI.Vector2IntField(position, GUIContent.none, array.Size);
			array.Size = newSize;
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
