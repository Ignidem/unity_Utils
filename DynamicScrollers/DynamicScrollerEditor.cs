#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;

namespace UnityUtils.DynamicScrollers.Editor
{
	[CustomEditor(typeof(DynamicScroller))]
	public class DynamicScrollerEditor : ScrollRectEditor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			for (int i = 0; i < DynamicScroller.serializedFields.Length; i++)
			{
				string name = DynamicScroller.serializedFields[i];
				SerializedProperty prop = serializedObject.FindProperty(name);
				if (prop == null) continue;
				EditorGUILayout.PropertyField(prop);
			}

			serializedObject.ApplyModifiedProperties();
			base.OnInspectorGUI();
		}
	}
}
#endif