using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

namespace UnityUtils.Editor.SerializedProperties
{
	public static class SerializedPropertyEx
	{
		private const BindingFlags BindingAttr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
			BindingFlags.Static;

		public static int GetIndex(this SerializedProperty property)
		{
			string path = property.propertyPath;
			if (path[^1] != ']') return -1;

			string num = string.Empty;
			for (int i = path.Length - 2; i > -1; i--)
			{
				char c = path[i];
				if (c == '[') break;
				num += c;
			}

			return int.TryParse(num, out int index) ? index : -1;
		}

		public static object GetTarget(this SerializedProperty prop) => prop.serializedObject.targetObject;

		public static object GetInstance(this SerializedProperty prop)
		{
			string[] path = prop.propertyPath.Replace(".Array.data[", "[").Split('.');
			return GetInstance(prop, path, path.Length);
		}

		public static object GetParent(this SerializedProperty prop)
		{
			string[] path = prop.propertyPath.Replace(".Array.data[", "[").Split('.');
			return GetInstance(prop, path, path.Length - 1);
		}

		private static object GetInstance(this SerializedProperty prop, string[] path, int maxDepth)
		{
			object target = GetTarget(prop);
			for (int depth = 0; depth < maxDepth; depth++)
			{
				string name = path[depth];
				target = ReadPath(name, target);
			}

			return target;
		}

		private static object ReadPath(string name, object target)
		{
			const char indexedEnd = ']';
			const string BackingField = nameof(BackingField);

			if (name.EndsWith(indexedEnd))
				return GetIndexed(name, target);

			if (name.EndsWith(BackingField))
				return GetProperty(name, target);

			return GetField(name, target);
		}

		private static object GetField(string name, object target)
		{
			Type type = target.GetType();
			FieldInfo member = type.GetField(name, BindingAttr);

			return member.GetValue(target);
		}

		private static object GetIndexed(string name, object target)
		{
			int start = name.IndexOf('[');
			int.TryParse(name[start..^1], out int index);
			object list = ReadPath(name[..start], target);
			return list switch
			{
				IList ilist => ilist[index],
				_ => throw new Exception("Unsupported Type " + list.GetType().Name)
			};
		}

		private static object GetProperty(string name, object target)
		{
			int start = name.IndexOf('<') + 1;
			int end = name.IndexOf('>');
			name = name[start..end];
			Type type = target.GetType();
			PropertyInfo member = type.GetProperty(name, BindingAttr);
			return member.GetValue(target);
		}
	}
}
