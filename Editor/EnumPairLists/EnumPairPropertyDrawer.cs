using UnityEditor;
using UnityEngine;
using UnityUtils.Editor;
using UnityUtils.Editor.SerializedProperties;
using UnityUtils.Storages.EnumPairLists;
using UnityUtils.RectUtils;

namespace UnityUtils.Storages.Editor.EnumPairLists
{
	[CustomPropertyDrawer(typeof(EnumPair<,>), true)]
	public class EnumPairPropertyDrawer : ExtendedPropertyDrawer
	{
		private string search;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			IEnumPairField enumpair = (IEnumPairField)property.GetInstance();
			SerializedProperty values = property.FindPropertyRelative(nameof(values));

			int count = enumpair.Count;
			if (values.arraySize < count)
				values.arraySize = count;

			position = position.MoveY(LineHeight);
			search = EditorGUI.TextField(position, search);

			for (int i = 0; i < count; i++)
			{
				string name = enumpair.GetNameAt(i);
				if (!string.IsNullOrEmpty(search) && !name.Contains(search, System.StringComparison.OrdinalIgnoreCase))
					continue;

				SerializedProperty valueX = values.GetArrayElementAtIndex(i);
				position = position.MoveY(LineHeight);
				EditorGUI.PropertyField(position, valueX, new GUIContent(name), true);
			}

			property.serializedObject.ApplyModifiedProperties();
			return 0;
		}
	}
}