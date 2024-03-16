using UnityEditor;
using UnityEngine;
using UnityUtils.AddressableUtils;
using UnityUtils.RectUtils;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(AddressableComponent<>))]
	public class AddressableComponentDrawer : AddressableReferenceDrawer
	{
		protected override System.Type GetPropertyFieldType()
		{
			return typeof(GameObject);
		}

		protected override bool ValidateAsset(ref Rect position, Object asset)
		{
			System.Type type = GetGenericType();
			if (asset is not GameObject go)
			{
				position = position.MoveY(SpacedLineHeight);
				EditorGUI.LabelField(position, $"<color=red>{asset.GetType().Name} {asset.name} is not a Game Object</color>");
				return false;
			}

			if (!go.GetComponentInChildren(type))
			{
				position = position.MoveY(SpacedLineHeight);
				string msg = (unityObjectType.IsAssignableFrom(type) ? "of type " : "implementing ") + type.Name;
				EditorGUI.LabelField(position, $"<color=red>Game Object does not have a component {msg}</color>");
				return false;
			}

			return true;
		}
	}
}
