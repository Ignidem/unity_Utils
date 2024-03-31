using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class ParticlesComponent : IVisualEffectComponents
	{
		[SerializeField]
		private ParticleSystem particles;
		public string Name => particles.gameObject.name;

		public T GetValue<T>(int id)
		{
			return particles.TryGetProperty(id, out T value) ? value : default;
		}

		public void SetValue<T>(int id, T value)
		{
			particles.TrySetProperty(id, value);
		}
	}
}
