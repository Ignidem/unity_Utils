using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityUtils.Effects.VisualEffects
{
	public static class MaterialPropertiesUtils
	{
		private static readonly Dictionary<Type, IPropertyDelegates> delegates = new()
		{
			[typeof(Color)] = new PropertyDelegates<Material, Color>(
				(comp, id) => comp.HasColor(id),
				(comp, id) => comp.GetColor(id),
				(comp, id, value) => comp.SetColor(id, value)
				),
			[typeof(float)] = new PropertyDelegates<Material, float>(
				(comp, id) => comp.HasFloat(id),
				(comp, id) => comp.GetFloat(id),
				(comp, id, value) => comp.SetFloat(id, value)
				),
			[typeof(int)] = new PropertyDelegates<Material, int>(
				(comp, id) => comp.HasInt(id),
				(comp, id) => comp.GetInt(id),
				(comp, id, value) => comp.SetInt(id, value)
				),
			[typeof(Vector2)] = new PropertyDelegates<Material, Vector2>(
				(comp, id) => comp.HasVector(id),
				(comp, id) => comp.GetVector(id),
				(comp, id, value) => comp.SetVector(id, value)
				),
			[typeof(Vector3)] = new PropertyDelegates<Material, Vector3>(
				(comp, id) => comp.HasVector(id),
				(comp, id) => comp.GetVector(id),
				(comp, id, value) => comp.SetVector(id, value)
				),
			[typeof(Vector4)] = new PropertyDelegates<Material, Vector4>(
				(comp, id) => comp.HasVector(id),
				(comp, id) => comp.GetVector(id),
				(comp, id, value) => comp.SetVector(id, value)
				),
		};

		public static T GetProperty<T>(this Material comp, int id)
		{
			if (!comp) return default;
			comp.TryGetProperty(id, out T value);
			return value;
		}

		public static T GetProperty<T>(this Material comp, string id)
		{
			if (!comp) return default;
			comp.TryGetProperty(id, out T value);
			return value;
		}

		public static bool TryGetProperty<T>(this Material comp, string name, out T value)
			=> comp.TryGetProperty(Shader.PropertyToID(name), out value);

		public static bool TryGetProperty<T>(this Material comp, int id, out T value)
		{
			if (!comp)
			{
				value = default;
				return false;
			}

			try
			{
				var prop = delegates.GetDelegates<Material, T>();
				value = prop.Get(comp, id);
				return true;
			}
			catch (MissingPropertyException mpe)
			{
				Debug.LogException(mpe);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}

			value = default;
			return false;
		}

		public static bool TrySetProperty<T>(this Material comp, string name, T value)
			=> comp.TrySetProperty(Shader.PropertyToID(name), value);

		public static bool TrySetProperty<T>(this Material comp, int id, T value)
		{
			if (!comp) return false;

			try
			{
				var prop = delegates.GetDelegates<Material, T>();
				prop.Set(comp, id, value);
				return true;
			}
			catch (MissingPropertyException mpe)
			{
				Debug.LogException(mpe);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}

			return false;
		}
	}
}
