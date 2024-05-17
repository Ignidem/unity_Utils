using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityUtils.Effects.VisualEffects
{
	public static class ParticlesPropertiesUtils
	{
		private static readonly Dictionary<Type, IPropertyDelegates> delegates = new()
		{
			/* Need to be defined properly.
			[typeof(Color)] = new PropertyDelegates<ParticleSystem, Color>(
				(comp, id) => comp.startColor,
				(comp, id) => comp.main.startColor,
				(comp, id, value) => comp.SetColor(id, value)
				),
			[typeof(float)] = new PropertyDelegates<ParticleSystem, float>(
				(comp, id) => comp.HasFloat(id),
				(comp, id) => comp.GetFloat(id),
				(comp, id, value) => comp.SetFloat(id, value)
				),
			[typeof(int)] = new PropertyDelegates<ParticleSystem, int>(
				(comp, id) => comp.HasInt(id),
				(comp, id) => comp.GetInt(id),
				(comp, id, value) => comp.SetInt(id, value)
				),
			[typeof(Vector2)] = new PropertyDelegates<ParticleSystem, Vector2>(
				(comp, id) => comp.HasVector(id),
				(comp, id) => comp.GetVector(id),
				(comp, id, value) => comp.SetVector(id, value)
				),
			[typeof(Vector3)] = new PropertyDelegates<ParticleSystem, Vector3>(
				(comp, id) => comp.HasVector(id),
				(comp, id) => comp.GetVector(id),
				(comp, id, value) => comp.SetVector(id, value)
				),
			[typeof(Vector4)] = new PropertyDelegates<ParticleSystem, Vector4>(
				(comp, id) => comp.HasVector(id),
				(comp, id) => comp.GetVector(id),
				(comp, id, value) => comp.SetVector(id, value)
				),
			*/
		};

		public static T GetProperty<T>(this ParticleSystem comp, int id)
		{
			if (!comp) return default;
			comp.TryGetProperty(id, out T value);
			return value;
		}

		public static T GetProperty<T>(this ParticleSystem comp, string id)
		{
			if (!comp) return default;
			comp.TryGetProperty(id, out T value);
			return value;
		}

		public static bool TryGetProperty<T>(this ParticleSystem comp, string name, out T value)
			=> comp.TryGetProperty(Shader.PropertyToID(name), out value);

		public static bool TryGetProperty<T>(this ParticleSystem comp, int id, out T value)
		{
			if (!comp)
			{
				value = default;
				return false;
			}

			try
			{
				var prop = delegates.GetDelegates<ParticleSystem, T>();
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

		public static bool TrySetProperty<T>(this ParticleSystem comp, string name, T value, bool log)
			=> comp.TrySetProperty(Shader.PropertyToID(name), value, log);

		public static bool TrySetProperty<T>(this ParticleSystem comp, int id, T value, bool log)
		{
			if (!comp) return false;

			try
			{
				var prop = delegates.GetDelegates<ParticleSystem, T>();
				prop.Set(comp, id, value);
				return true;
			}
			catch (MissingPropertyException mpe)
			{
				if (log) Debug.LogException(mpe);
			}
			catch (Exception e)
			{
				if (log) Debug.LogException(e);
			}

			return false;
		}
	}
}
