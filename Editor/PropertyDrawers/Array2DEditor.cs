using UnityUtils.Editor.SerializedProperties;
using System.Collections.Generic;
using UnityUtils.RectUtils;
using UnityEditor;
using UnityEngine;
using UnityUtils.Arrays;

namespace UnityUtils.Editor
{
	[CustomPropertyDrawer(typeof(Array2D<>), true)]
	public class Array2DEditor : ExtendedPropertyDrawer
	{
		private const string ElementsField = Array2D<object>.ElementsFieldName;
		private const string SizeField = nameof(Array2D<object>.Size);

		private Vector2Int size;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			IArray2D target = property.GetInstance() as IArray2D;
			position = DrawSize(position, target);
			property.serializedObject.ApplyModifiedProperties();
			position = DrawGrid(position, property, target);
			property.serializedObject.ApplyModifiedProperties();

			return 0;
		}

		private Rect DrawSize(Rect position, IArray2D array)
		{
			position = position.MoveY(SpacedLineHeight).SetHeight(LineHeight);
			array.Size = EditorGUI.Vector2IntField(position, GUIContent.none, array.Size);
			return position;
		}

		private Rect DrawGrid(Rect position, SerializedProperty prop, IArray2D array)
		{
			Vector2Int size = array.Size;
			float width = Mathf.Max(10, (ViewWidth - IndentWidth) / size.x);
			position = position.MoveY(LineHeight).SetX(IndentWidth / 2)
				.SetWidth(width).SetHeight(LineHeight);

			SerializedProperty list = prop.FindPropertyRelative(ElementsField);
			Rect pos = position;
			for (int x = 0; x < size.x; x++)
			{
				SerializedProperty subList = list.GetArrayElementAtIndex(x).FindPropertyRelative(ElementsField);
				if (subList == null) continue;

				for (int y = 0; y < size.y; y++)
				{
					SerializedProperty element = subList.GetArrayElementAtIndex(y);
					if (element == null) continue;
					pos = position.Move(new Vector2(x * width, y * LineHeight));
					EditorGUI.PropertyField(pos, element, GUIContent.none);
				}
			}

			return pos;
		}
	}
}
