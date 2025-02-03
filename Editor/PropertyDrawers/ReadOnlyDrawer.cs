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

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			ReadOnlyAttribute attr = (ReadOnlyAttribute)attribute;
			float xOffset = 0;
			if (attr.hasToggle)
			{
				Rect unlockRect = position.SetSize(LineHeight, LineHeight);
				isUnlocked = EditorGUI.Toggle(unlockRect, isUnlocked);
				xOffset = LineHeight;
			}

			EditorGUI.BeginDisabledGroup(!isUnlocked);
			EditorGUI.PropertyField(position.MoveX(xOffset), property);
			EditorGUI.EndDisabledGroup();

			return 0;
		}
	}
}
