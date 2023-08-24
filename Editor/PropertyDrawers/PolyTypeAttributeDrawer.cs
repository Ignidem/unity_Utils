using UnityEditor;
using UnityEngine;
using UnityUtils.Editor.SerializedProperties;
using UnityUtils.PropertyAttributes;
using UnityUtils.RectUtils;

namespace UnityUtils.Editor.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(PolyTypeAttribute))]
	public class PolyTypeDrawer : ExtendedPropertyDrawer
	{
		protected override void OnFirstGUI(SerializedProperty prop)
		{
			if (attribute is PolyTypeAttribute poly)
			{
				poly.SetFieldInfo(prop.GetParent(), fieldInfo);
			}
		}

		protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
		{
			PolyTypeDropdown(position);
			position = position.MoveY(SpacedLineHeight);
			return DefaultGUI(ref position, property);
		}

		private Rect PolyTypeDropdown(Rect position)
		{
			if (attribute is not PolyTypeAttribute poly) return position;

			poly.Index = EditorGUI.Popup(position.SetHeight(LineHeight), poly.Index, poly.options);

			return position;
		}
	}
}
