using UnityEditor;
using UnityEngine;
using UnityUtils.Editor.SerializedProperties;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.Storages.Editor.EnumPairLists
{
	[CustomPropertyDrawer(typeof(EnumPair<,>), true)]
	public class EnumPairPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			IEnumPairField enumpair = (IEnumPairField)property.GetInstance();
			SerializedProperty values = property.FindPropertyRelative(nameof(values));

			int count = enumpair.Count;
			if (values.arraySize < count)
				values.arraySize = count;

			for (int i = 0; i < count; i++)
			{
				string name = enumpair.GetNameAt(i);
				SerializedProperty valueX = values.GetArrayElementAtIndex(i);
				EditorGUILayout.PropertyField(valueX, new GUIContent(name), true);
			}

			property.serializedObject.ApplyModifiedProperties();
		}
	}
}