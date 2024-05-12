using System;
using UnityEngine;
using UnityUtils.Effects.VisualEffects.ParameterFunctions;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Effects.VisualEffects
{
	public interface ILightParameterFunctions : IParameterFunctions<Light> { }

	[Serializable]
	public struct LightComponent : IVisualEffectComponent
	{
		[SerializeField] private Light light;

		[SerializeReference, Polymorphic(true)]
		private ILightParameterFunctions functions;

		public readonly string Name => light.name;

		public readonly void SetValue<T>(int id, T value) 
		{
			functions?.SetValue(light, id, value);
		}
		public readonly T GetValue<T>(int id)
		{
			return functions != null ? functions.GetValue<T>(light, id) : default;
		}

		public readonly void Play()
		{
			light.enabled = true;
		}

		public readonly void Stop()
		{
			light.enabled = false;
		}
	}
}
