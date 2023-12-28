#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Serialization.Editor
{
	[CustomPropertyDrawer(typeof(Serialized.Dictionary<,>), true)]
	public class DictionaryDrawer : PropertyDrawer
	{
		private const string Pairs = "pairs";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty pairs = property.FindPropertyRelative(Pairs);
			EditorGUI.PropertyField(position, pairs, label, true);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializedProperty pairs = property.FindPropertyRelative(Pairs);
			return EditorGUI.GetPropertyHeight(pairs, true);
		}
	}
}
#endif