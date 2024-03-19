using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityUtils.RectUtils;

namespace UnityUtils.Editor
{
	public class PropertyHandler
	{
		private static readonly MethodInfo GetHandler;
		private static readonly MethodInfo OnGUI;
		private static readonly MethodInfo GetDrawerTypeForPropertyAndType;
		private static readonly MethodInfo GetDrawerTypeForType;
		static PropertyHandler()
		{
			Type scriptAttributeUtility = Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor");
			GetHandler = scriptAttributeUtility
				.GetMethod(nameof(GetHandler), BindingFlags.NonPublic | BindingFlags.Static);
			Type propertyHandler = Type.GetType("UnityEditor.PropertyHandler, UnityEditor");
			OnGUI = propertyHandler.GetMethod(nameof(OnGUI), BindingFlags.Public | BindingFlags.Instance);
			GetDrawerTypeForType = scriptAttributeUtility.GetMethod(nameof(GetDrawerTypeForType),
				BindingFlags.NonPublic | BindingFlags.Static);
			GetDrawerTypeForPropertyAndType = scriptAttributeUtility.GetMethod(nameof(GetDrawerTypeForPropertyAndType),
				BindingFlags.NonPublic | BindingFlags.Static);
		}

		public bool IsValid => drawer != null;

		public Type DrawerType => drawer?.GetType();

		private SerializedProperty property;
		private readonly PropertyDrawer drawer;

		public PropertyHandler(SerializedProperty property, Type type)
		{
			this.property = property;

			if (type == null)
				return;

			Type drawerType = (Type)GetDrawerTypeForPropertyAndType.Invoke(null, new object[] { property, type });
			if (drawerType != null)
				drawer = (PropertyDrawer)Activator.CreateInstance(drawerType);
		}

		public PropertyHandler(Type type)
		{
			Type drawerType = (Type)GetDrawerTypeForType.Invoke(null, new object[] { type });
			if (drawerType != null)
				drawer = (PropertyDrawer)Activator.CreateInstance(drawerType);
		}

		public Rect DrawProperty(Rect position, SerializedProperty property)
		{
			this.property = property;
			return DrawProperty(position);
		}

		public Rect DrawProperty(Rect position)
		{
			return DrawerOnGUI(position);
		}

		public Rect DrawerOnGUI(Rect position)
		{
			GUIContent label = new GUIContent(property.displayName);
#pragma warning disable UNT0027 // Do not call PropertyDrawer.OnGUI()
			drawer.OnGUI(position, property, label);
			float height = drawer.GetPropertyHeight(property, label);
			return position.MoveY(height);
#pragma warning restore UNT0027 // Do not call PropertyDrawer.OnGUI()
		}
	}
}
