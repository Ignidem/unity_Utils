using UnityEditor;
using UnityEngine;
using UnityUtils.RectUtils;
using UnityUtils.AddressableUtils;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

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

			//Get GUID Property
			SerializedProperty asset = property.FindPropertyRelative(AddressableReference<Component>.FieldName);
			SerializedProperty guidProp = asset.FindPropertyRelative(AssetGUID);

			//cache guid value;
			string guid = guidProp.stringValue;

			//Find addressable object;
			Object current = string.IsNullOrEmpty(guid) ? null :
				AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), objectType);

			//Editor Object Field of generic type (component or object)
			GUIContent _label = new GUIContent(property.displayName);
			Object result = EditorGUI.ObjectField(position, _label, current, objectType, false);
			//Check is field changed;
			if (current == result) return 0;

			//Get new guid value;
			guid = !current ? string.Empty : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(result));
			guidProp.stringValue = guid;

			if (!current) return 0;

			AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
			//Check is field value is already addressable;
			AddressableAssetEntry entry = settings.FindAssetEntry(guid);
			if (entry == null)
			{
				//Make object addressable;
				settings.CreateAssetReference(guid);
			}

			return 0;
		}
	}
}
