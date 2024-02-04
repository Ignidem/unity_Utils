using UnityEditor;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.Storages.Editor.EnumPairLists
{
	[CustomEditor(typeof(EnumPairObject<,>), true)]
	public class EnumPairObjectEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			SerializedProperty pairs = serializedObject.FindProperty(nameof(pairs));
			EditorGUILayout.PropertyField(pairs);
		}
	}
}