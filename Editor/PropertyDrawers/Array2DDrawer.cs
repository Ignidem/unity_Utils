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

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			IArray2D target = property.GetInstance() as IArray2D;
			VisualElement container = new VisualElement();
			DrawSize(container, target);

			Vector2Int size = target.Size;
			if (size.x != 0 && size.y != 0)
				DrawGrid(container, property, target.Size);

			return container;
		}

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			IArray2D target = property.GetInstance() as IArray2D;
			DrawSize(ref position, target);

			Vector2Int size = target.Size;
			if (size.x != 0 && size.y != 0)
				position = DrawGrid(position, property, size);

			property.serializedObject.ApplyModifiedProperties();
			return 0;
		}

		private void DrawSize(VisualElement container, IArray2D array)
		{
			Vector2IntField sizeField = new Vector2IntField
			{
				value = array.Size
			};
			void OnChangeSize(ChangeEvent<Vector2Int> evt)
			{
				array.Size = evt.newValue;
			}

			sizeField.RegisterCallback<ChangeEvent<Vector2Int>>(OnChangeSize);
			container.Add(sizeField);
		}

		private void DrawSize(ref Rect position, IArray2D array)
		{
			position = position.SetHeight(LineHeight);
			Vector2Int newSize = EditorGUI.Vector2IntField(position, GUIContent.none, array.Size);
			array.Size = newSize;
		}

		private void DrawGrid(VisualElement container, SerializedProperty property, Vector2Int size)
		{
			SerializedProperty list = property.FindPropertyRelative(ElementsField);

			void OnElement(SerializedProperty element, int x, int y)
			{
				PropertyField field = DrawCell(element);
				container.Add(field);
			}

			Loop(size, list, OnElement);
		}

		protected virtual Rect DrawGrid(Rect position, SerializedProperty prop, Vector2Int size)
		{
			SerializedProperty list = prop.FindPropertyRelative(ElementsField);

			float width = Mathf.Max(10, (ViewWidth - IndentWidth) / size.x);
			position = position.MoveY(LineHeight).SetX(IndentWidth / 2)
				.SetWidth(width).SetHeight(LineHeight);

			Rect pos = position;
			void OnElement(SerializedProperty element, int x, int y)
			{
				pos = position.Move(new Vector2(x * width, y * LineHeight));
				DrawCell(pos, element);
			}

			Loop(size, list, OnElement);

			return pos;
		}

		private void Loop(Vector2Int size, SerializedProperty list, Action<SerializedProperty, int, int> onElement)
		{
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

					onElement(element, x, y);
				}
			}
		}

		protected virtual void DrawCell(Rect pos, SerializedProperty element)
		{
			EditorGUI.PropertyField(pos, element, GUIContent.none);
		}

		protected virtual PropertyField DrawCell(SerializedProperty element)
		{
			return new PropertyField(element, null);
		}
	}
}
