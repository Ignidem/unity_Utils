using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class ParticlesComponent : IVisualEffectComponent
	{
		[SerializeField]
		private ParticleSystem particles;
		public string Name => particles.gameObject.name;

		public ParticleSystem.MinMaxCurve StartSize
		{
			get => particles.main.startSize;
			set
			{
				var module = particles.main;
				module.startSize = value;
			}
		}
		public ParticleSystem.MinMaxCurve RateOverTime
		{
			get => particles.emission.rateOverTime;
			set
			{
				ParticleSystem.EmissionModule emission = particles.emission;
				emission.rateOverTime = value;
			}
		}
		public Vector3 Velocity
		{
			get
			{
				var v = particles.velocityOverLifetime;
				return new Vector3(v.x.constant, v.y.constant, v.z.constant);
			}
			set
			{
				var v = particles.velocityOverLifetime;
				v.x = value.x;
				v.y = value.y;
				v.z = value.z;
			}
		}
		public ParticleSystem.ShapeModule Area
		{
			get => particles.shape;
		}

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
