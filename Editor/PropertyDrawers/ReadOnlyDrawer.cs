using UnityEditor;
using UnityEngine;
using UnityUtils.PropertyAttributes;
using UnityUtils.RectUtils;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyDrawer : ExtendedPropertyDrawer
	{
		protected override LabelDrawType LabelType => LabelDrawType.None;

		private bool isUnlocked;
		private GUIStyle richTextStyle;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			richTextStyle ??= new GUIStyle(EditorStyles.label)
			{
				richText = true
			};
			
			ReadOnlyAttribute attr = (ReadOnlyAttribute)attribute;

			float xOffset = 0;
			if (attr.HasToggle)
			{
				Rect unlockRect = position.SetSize(LineHeight, LineHeight);
				isUnlocked = EditorGUI.Toggle(unlockRect, isUnlocked);
				xOffset = LineHeight;
			}

			Rect moveX = position.MoveX(xOffset);
			if (attr.AsLabel && !isUnlocked)
			{
				EditorGUI.LabelField(moveX, property.stringValue, richTextStyle);
			}
			else
			{
				EditorGUI.BeginDisabledGroup(!isUnlocked);
				EditorGUI.PropertyField(moveX, property);
				EditorGUI.EndDisabledGroup();
			}

			return 0;
		}
	}
}
