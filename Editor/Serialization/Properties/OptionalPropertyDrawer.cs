using UnityEditor;
using UnityEngine;
using UnityUtils.RectUtils;
using UnityUtils.Serialization.Properties;

namespace UnityUtils.Editor.Serialization.Properties
{
	[CustomPropertyDrawer(typeof(Optional<>), true)]
	public class OptionalPropertyDrawer : ExtendedPropertyDrawer
	{
		protected override LabelDrawType LabelType => LabelDrawType.None;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty enabledProp = property.FindPropertyRelative(nameof(Optional<object>.enabled));
			SerializedProperty valueProp = property.FindPropertyRelative(nameof(Optional<object>.value));

			float xOffset = 0;
			Rect unlockRect = position.SetSize(LineHeight, LineHeight);
			enabledProp.boolValue = EditorGUI.Toggle(unlockRect, enabledProp.boolValue);
			xOffset = LineHeight;

			EditorGUI.BeginDisabledGroup(!enabledProp.boolValue);
			EditorGUI.PropertyField(position.MoveX(xOffset), valueProp, label);
			EditorGUI.EndDisabledGroup();

			return 0;
		}
	}
}