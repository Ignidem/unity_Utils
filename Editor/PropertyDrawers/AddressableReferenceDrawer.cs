using UnityEditor;
using UnityEngine;
using UnityUtils.RectUtils;
using UnityUtils.AddressableUtils;
using UnityEngine.AddressableAssets;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(AddressableReference<>))]
	public class AddressableReferenceDrawer : ExtendedPropertyDrawer
	{
		private const string AssetGUID = "m_AssetGUID";
		static readonly System.Type unityObjectType = typeof(Object);

		protected override LabelDrawType LabelType => LabelDrawType.None;

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			System.Type fieldType = fieldInfo.FieldType;
			System.Type genericType = fieldType.IsArray ? fieldType.GetElementType() : fieldType;
			System.Type objectType = genericType.GenericTypeArguments[0];

			if (!unityObjectType.IsAssignableFrom(objectType))
			{
				return 0;
			}

			SerializedProperty asset = property.FindPropertyRelative(AddressableReference<int>.FieldName);
			SerializedProperty guid = asset.FindPropertyRelative(AssetGUID);

			Object current = string.IsNullOrEmpty(guid.stringValue) ? null :
				AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid.stringValue), objectType);

			GUIContent _label = new GUIContent(property.displayName);
			Object result = EditorGUI.ObjectField(position, _label, current, objectType, false);

			if (current != result)
			{
				guid.stringValue = !current ? string.Empty :
					AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(result));
			}

			return 0;
		}
	}
}
