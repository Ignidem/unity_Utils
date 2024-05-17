using System;
using UnityEngine;
using UnityUtils.Effects.VisualEffects.ParameterFunctions;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IRendererParameterFunctions : IParameterFunctions<Renderer> { }

	[Serializable]
	public struct RendererComponent : IVisualEffectComponent
	{
		[SerializeField] private Renderer render;

		[SerializeReference, Polymorphic(true)]
		private IRendererParameterFunctions functions;

		public readonly string Name => render.gameObject.name;
		private readonly Material Material => render.material;

		public readonly void Play()
		{
			render.gameObject.SetActive(true);
		}

		public readonly void Stop()
		{
			render.gameObject.SetActive(false);
		}

		public readonly T GetValue<T>(int id)
		{
			return functions != null ? functions.GetValue<T>(render, id)
				: Material.TryGetProperty(id, out T value) ? value : default;
		}

		public readonly void SetValue<T>(int id, T value, bool isOptional)
		{
			if (functions != null)
				functions.SetValue(render, id, value, isOptional);
			else 
				Material.TrySetProperty(id, value, !isOptional);
		}
	}
}
