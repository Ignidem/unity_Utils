using UnityEditor;
using UnityEngine;
using UnityUtils.AddressableUtils;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(AddressableLoader))]
	public class AsyncAssetPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty asset = property.FindPropertyRelative(AddressableLoader.FieldName);
			EditorGUI.PropertyField(position, asset, label);
		}
	}
}
