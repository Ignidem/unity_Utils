using System;
using UnityEngine;
using UnityUtils.Effects.VisualEffects.ParameterFunctions;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IParticlesParameterFunctions : IParameterFunctions<ParticlesComponent> { }

	[Serializable]
	public class ParticlesComponent : IVisualEffectComponent
	{
		[SerializeField]
		private ParticleSystem particles;

		[SerializeReference, Polymorphic(true)]
		private IParticlesParameterFunctions functions;

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
		public Vector3 Position
		{
			get => Area.position;
			set
			{
				var area = Area;
				area.position = value;
			}
		}
		public Vector3 Rotation
		{
			get => Area.rotation;
			set
			{
				var area = Area;
				area.rotation = value;
			}
		}
		public Vector3 Scale
		{
			get => Area.scale;
			set
			{
				var area = Area;
				area.scale = value;
			}
		}
		public float Radius
		{
			get => Area.radius;
			set
			{
				var area = Area;
				area.radius = value;
			}
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
			if (functions != null)
				return functions.GetValue<T>(this, id);

			return particles.TryGetProperty(id, out T value) ? value : default;
		}

		public void SetValue<T>(int id, T value, bool isOptional = false)
		{
			if (functions != null)
			{
				functions.SetValue(this, id, value, isOptional);
				return;
			}
		}
	}
}
