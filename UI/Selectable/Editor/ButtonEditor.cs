using UnityEditor;
using UnityUtils.Editor.SerializedProperties;

namespace UnityUtils.UI.Selectable.Editor
{
	[CustomEditor(typeof(Button), true)]
	public class ButtonEditor : UnityEditor.UI.ButtonEditor
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.PropertyField(serializedObject.GetRelativeProperty(nameof(Button.Id)));
			EditorGUILayout.PropertyField(serializedObject.GetRelativeProperty(nameof(Button.IsToggle)));
			EditorGUILayout.PropertyField(serializedObject.GetRelativeProperty("animations"));
			serializedObject.ApplyModifiedProperties();
			base.OnInspectorGUI();
		}
	}
}
