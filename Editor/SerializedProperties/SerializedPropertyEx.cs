using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Editor.SerializedProperties
{
	public static class SerializedPropertyEx
	{
		private const BindingFlags BindingAttr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
			BindingFlags.Static | BindingFlags.FlattenHierarchy;
		private const string backingField = "<{0}>k__BackingField";

		public static bool IsBoxedValueValid(this SerializedProperty prop)
		{
			if (prop.propertyType != SerializedPropertyType.Generic && !prop.isArray)
				return true;

			try
			{
				_ = prop.boxedValue;
				return true;
			}
			catch { }

			return false;
		}
		public static bool TryGetBoxedValue<T>(this SerializedProperty prop, out T value)
		{
			try
			{
				if (prop.boxedValue is T _t)
				{
					value = _t;
					return true;
				}
			}
			catch { }

			value = default;
			return false;
		}

		public static SerializedProperty GetRelativeProperty(this SerializedObject obj, string name)
		{
			return obj.FindProperty(name) ?? obj.FindProperty(string.Format(backingField, name));
		}

		public static SerializedProperty GetRelativeProperty(this SerializedProperty prop, string name)
		{
			return prop.FindPropertyRelative(name) ?? prop.FindPropertyRelative(string.Format(backingField, name));
		}

		public static Type GetValueType(this SerializedProperty prop)
		{
			return prop.propertyType switch
			{
				SerializedPropertyType.Integer => typeof(int),
				SerializedPropertyType.ArraySize => typeof(int),
				SerializedPropertyType.Boolean => typeof(bool),
				SerializedPropertyType.Float => typeof(float),
				SerializedPropertyType.String => typeof(string),
				SerializedPropertyType.Color => typeof(Color),
				SerializedPropertyType.LayerMask => typeof(LayerMask),
				SerializedPropertyType.Vector2 => typeof(Vector2),
				SerializedPropertyType.Vector3 => typeof(Vector3),
				SerializedPropertyType.Vector4 => typeof(Vector4),
				SerializedPropertyType.Rect => typeof(Rect),
				SerializedPropertyType.Character => typeof(char),
				SerializedPropertyType.AnimationCurve => typeof(AnimationCurve),
				SerializedPropertyType.Bounds => typeof(Bounds),
				SerializedPropertyType.Gradient => typeof(Gradient),
				SerializedPropertyType.Quaternion => typeof(Quaternion),
				SerializedPropertyType.Vector2Int => typeof(Vector2Int),
				SerializedPropertyType.Vector3Int => typeof(Vector3Int),
				SerializedPropertyType.RectInt => typeof(RectInt),
				SerializedPropertyType.BoundsInt => typeof(BoundsInt),
				SerializedPropertyType.Hash128 => typeof(Hash128),

				SerializedPropertyType.ObjectReference => prop.objectReferenceValue.GetType(),
				SerializedPropertyType.Generic when prop.IsBoxedValueValid() => prop.boxedValue.GetType(),
				SerializedPropertyType.ExposedReference => prop.exposedReferenceValue.GetType(),
				SerializedPropertyType.ManagedReference when prop.managedReferenceValue != null => prop.managedReferenceValue.GetType(),
				SerializedPropertyType.FixedBufferSize => typeof(int),
				SerializedPropertyType.Enum => typeof(Enum),
				_ => prop.GetFieldType(prop.isArray)
			};
		}

		public static bool IsArrayElement(this SerializedProperty property)
		{
			return property.propertyPath[^1] == ']';
		}

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

		public static UnityEngine.Object GetTarget(this SerializedProperty prop) => prop.serializedObject.targetObject;

		public static object GetInstance(this SerializedProperty prop)
		{
			if (prop.TryGetBoxedValue(out object value))
				return value;
			
			string[] path = prop.propertyPath.Replace(".Array.data[", "[").Split('.');
			return GetInstance(prop, path, path.Length);
		}

		public static Type GetFieldType(this SerializedProperty prop, bool elementType)
		{
			object parent = prop.GetParent();
			Type pt = parent.GetType();
			Type type = pt.GetField(prop.name, BindingAttr)?.FieldType ?? pt.GetProperty(prop.name, BindingAttr)?.PropertyType;
			return !elementType ? type
				: type.IsArray ? type.GetElementType()
				: type.IsGenericType ? type.GetGenericArguments()[0]
				: type;
		}

		public static object GetParent(this SerializedProperty prop)
		{
			/*
			string path = prop.propertyPath;
			if (path[^1] == ']') path = path[..path.LastIndexOf("Array.data")];
			int parentIndex = path.LastIndexOf('.');
			if (parentIndex != -1) path = path[..parentIndex];
			SerializedProperty parentProp = prop.serializedObject.FindProperty(path);
			if (parentProp.TryGetBoxedValue(out object value))
				return value;
			*/

			string[] paths = prop.propertyPath.Replace(".Array.data[", "[").Split('.');
			return GetInstance(prop, paths, paths.Length - 1);
		}

		private static object GetInstance(this SerializedProperty prop, string[] path, int maxDepth)
		{
			UnityEngine.Object _target = GetTarget(prop);
			
			object target = _target;
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
			string sint = name[(start + 1)..^1];
			if (!int.TryParse(sint, out int index))
			{
				throw new Exception("Failed to int parse " + sint + " from " + name);
			}

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
