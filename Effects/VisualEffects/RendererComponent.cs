using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class RendererComponent : IVisualEffectComponents
	{
		[SerializeField]
		private Renderer render;
		public string Name => render.gameObject.name;

		private Material Material => render.material;

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
