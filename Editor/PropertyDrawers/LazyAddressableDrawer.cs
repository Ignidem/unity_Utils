using UnityEditor;
using UnityEngine;
using UnityUtils.AddressableUtils;
using UnityUtils.RectUtils;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(LazyAddressable<>))]
	public class LazyAddressableDrawer : AddressableReferenceDrawer
	{
		protected override LabelDrawType LabelType => LabelDrawType.Foldout;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			position = position.MoveY(LineHeight);
			return base.DrawProperty(ref position, property, label) 
				+ DrawParentField(ref position, property, label);
		}

		private float DrawParentField(ref Rect position, SerializedProperty property, GUIContent label)
		{
			position = position.MoveY(SpacedLineHeight);
			EditorGUI.PropertyField(position, property.FindPropertyRelative("parent"));
			return 0;
		}
	}
}
