using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class RendererComponent : IVisualEffectComponent
	{
		[SerializeField]
		private Renderer render;
		public string Name => render.gameObject.name;

		private Material Material => render.material;

		public void Play()
		{
			render.gameObject.SetActive(true);
		}

		public void Stop()
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
