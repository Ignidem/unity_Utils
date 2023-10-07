using UnityUtils.Editor.SerializedProperties;
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

		protected override LabelDrawType LabelType => LabelDrawType.None;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			IArray2D target = property.GetInstance() as IArray2D;
			if (DrawSize(ref position, target))
				property.serializedObject.ApplyModifiedProperties();

			position = DrawGrid(position, property, target);
			property.serializedObject.ApplyModifiedProperties();

			return 0;
		}

		private bool DrawSize(ref Rect position, IArray2D array)
		{
			position = position.SetHeight(LineHeight);
			Vector2Int newSize = EditorGUI.Vector2IntField(position, GUIContent.none, array.Size);
			bool shouldUpdate = newSize != array.Size;
			array.Size = newSize;
			return shouldUpdate;
		}

		private Rect DrawGrid(Rect position, SerializedProperty prop, IArray2D array)
		{
			Vector2Int size = array.Size;

			if (size.x == 0 || size.y == 0) return position;

			SerializedProperty list = prop.FindPropertyRelative(ElementsField);
			float width = Mathf.Max(10, (ViewWidth - IndentWidth) / size.x);
			position = position.MoveY(LineHeight).SetX(IndentWidth / 2)
				.SetWidth(width).SetHeight(LineHeight);
			Rect pos = position;
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
					pos = position.Move(new Vector2(x * width, y * LineHeight));
					EditorGUI.PropertyField(pos, element, GUIContent.none);
				}
			}

			return pos;
		}
	}
}
