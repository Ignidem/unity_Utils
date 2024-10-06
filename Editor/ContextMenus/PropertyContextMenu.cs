using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityUtils.Editor.SerializedProperties;
using Utilities.Reflection;

namespace UnityUtils.Editor.ContextMenus
{
	[InitializeOnLoad]
	public class PropertyContextMenu
	{
		private static Dictionary<Type, IPropertyContextMenu> menuBuilders;

		static PropertyContextMenu()
		{
			EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
			menuBuilders = typeof(IPropertyContextMenu).GetSubTypes()
				.Select(t => (type: t, attr: t.GetCustomAttribute(typeof(PropertyContextMenuAttribute), false)))
				.Where(ta => ta.attr is PropertyContextMenuAttribute)
				.ToDictionary(
					ta => (ta.attr as PropertyContextMenuAttribute).propertyType, 
					ta => (IPropertyContextMenu)Activator.CreateInstance(ta.type)
				);
		}
		private static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			Type type;
			try
			{
				type = property.GetValueType();
				foreach (Type t in menuBuilders.Keys)
				{
					if (!type.Inherits(t))
						continue;

					IPropertyContextMenu value = menuBuilders[t];
					value.AddPropertyContextMenu(menu, property); 
				}
			}
			catch
			{
				return;
			}

			if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue is MonoScript script)
			{
				EditScriptMenuOption(menu, script);
				return;
			}

			if (type != null && !type.IsPrimitive)
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
