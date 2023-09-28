using UnityEditor;
using UnityEditor.UI;

namespace UnityUtils.DynamicScrollers.Editor
{
	[CustomEditor(typeof(DynamicScroller))]
	public class DynamicScrollerEditor : ScrollRectEditor
	{
		public override void OnInspectorGUI()
		{
			for (int i = 0; i < DynamicScroller.serializedFields.Length; i++)
			{
				string name = DynamicScroller.serializedFields[i];
				SerializedProperty prop = serializedObject.FindProperty(name);
				EditorGUILayout.PropertyField(prop);
			}

			base.OnInspectorGUI();
		}
	}
}
