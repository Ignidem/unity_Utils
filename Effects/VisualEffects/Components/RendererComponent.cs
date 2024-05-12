using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{

	[Serializable]
	public struct RendererComponent : IVisualEffectComponent
	{
		[SerializeField]
		private Renderer render;
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

		public T GetValue<T>(int id)
		{
			return Material.TryGetProperty(id, out T value) ? value : default;
		}

		public void SetValue<T>(int id, T value)
		{
			Material.TrySetProperty(id, value);
		}
	}
}
