using System;
using UnityEngine;
using UnityUtils.Effects.VisualEffects.ParameterFunctions;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IAudioParameterFunctions : IParameterFunctions<AudioSource> { }

	[Serializable]
	public struct AudioComponent : IVisualEffectComponent
	{
		[SerializeField]
		private AudioSource source;

		[SerializeReference, Polymorphic(true)]
		private IAudioParameterFunctions functions;

		public readonly string Name => source.clip.name;

		public readonly void Play()
		{
			source.enabled = true;
			source.Play();
		}
		public readonly void Stop()
		{
			source.enabled = false;
		}

		public readonly T GetValue<T>(int id)
		{
			return functions == null ? default : functions.GetValue<T>(source, id);
		}
		public readonly void SetValue<T>(int id, T value, bool isOptional = false)
		{
			functions?.SetValue(source, id, value, isOptional);
		}
	}
}
