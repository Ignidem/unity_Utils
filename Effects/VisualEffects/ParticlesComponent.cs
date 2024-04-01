using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class ParticlesComponent : IVisualEffectComponents
	{
		[SerializeField]
		private ParticleSystem particles;
		public string Name => particles.gameObject.name;

		public void Play()
		{
			particles.gameObject.SetActive(true);

			if (!particles.isPlaying)
				particles.Play(true);
		}

		public void Stop()
		{
			if (!particles.isPlaying)
				particles.Stop();

			particles.gameObject.SetActive(false);
		}

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
