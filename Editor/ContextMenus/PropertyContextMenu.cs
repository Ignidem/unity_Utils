using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityUtils.Editor.SerializedProperties;

namespace UnityUtils.Editor.ContextMenus
{
	public static class PropertyContextMenu
	{
		private static bool wasSubbed;

		public static void Enable()
		{
			if (wasSubbed)
				return;

			EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
			wasSubbed = true;
		}

		private static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			if (property.objectReferenceValue is MonoScript script)
			{
				EditScriptMenuOption(menu, script);
				return;
			}

			Type type = property.GetValueType();
			if (type == null || type.IsPrimitive)
				return;

			TypeContextMenu(menu, type);
		}

		private static void TypeContextMenu(GenericMenu context, Type type)
		{
			if (type == null || !TryGetScript(type, out MonoScript script))
				return;

			EditScriptMenuOption(context, script);
		}

		private static void EditScriptMenuOption(GenericMenu context, MonoScript script)
		{
			context.AddItem(new GUIContent("Edit " + script.name), false, () => AssetDatabase.OpenAsset(script));
		}

		private static bool TryGetScript(Type type, out MonoScript script)
		{
			string[] guids = AssetDatabase.FindAssets("t:script " + type.Name, new[] { "Assets" });

			static MonoScript GuidToScript(string guid)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);
				return AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
			}

			bool IsType(MonoScript script)
			{
				return script.GetClass() == type;
			}

			return script = guids.Select(GuidToScript).FirstOrDefault(IsType);
		}
	}
}
