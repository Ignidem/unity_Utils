using UnityEditor;
using UnityEngine;
using UnityUtils.AddressableUtils;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(AddressableReference<>), true)]
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
			SerializedProperty assetProp = property.FindPropertyRelative(AddressableReference<Component>.FieldName);
			SerializedProperty guidProp = assetProp.FindPropertyRelative(AssetGUID);

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
			guid = !result ? string.Empty : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(result));
			guidProp.stringValue = guid;

			if (!result) return UpdateInfo(property, null);

			AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
			//Check is field value is already addressable;
			AddressableAssetEntry entry = settings.FindAssetEntry(guid);

			AssetReference asset = entry == null ? settings.CreateAssetReference(guid) : new AssetReference(guid);
			return UpdateInfo(property, asset);
		}

		private int UpdateInfo(SerializedProperty property, AssetReference asset)
		{
			const string backingFieldFormat = "<{0}>k__BackingField";

			string name = string.Format(backingFieldFormat, nameof(AddressableReference<Component>.Name));
			SerializedProperty nameProp = property.FindPropertyRelative(name);
			string path = string.Format(backingFieldFormat, nameof(AddressableReference<Component>.Path));
			SerializedProperty keyProp = property.FindPropertyRelative(path);

			keyProp.stringValue = asset?.RuntimeKey?.ToString();
			nameProp.stringValue = asset != null && asset.editorAsset ? asset.editorAsset.name : null;

			return 0;
		}
	}
}
