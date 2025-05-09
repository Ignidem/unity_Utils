using System;
using DG.Tweening;
using UnityEngine;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Effects.VisualEffects
{
	public class AudioEffect : EffectComponent
	{
		[SerializeField]
		private AudioSource source;
		
		[SerializeReference, Polymorphic(true)]
		private IAudioParameterFunctions audioFunctions;

		public void FadeVolume(float volume)
		{
			DOTween.To(() => source.volume, (v) => source.volume = v, volume, 1);
		}
		
		public override void Play()
		{
			source.enabled = true;
			if (source.gameObject.activeInHierarchy)
				source.Play();
		}

		public override void Stop()
		{
			source.enabled = false;
		}

		public override T GetValue<T>(int id)
		{
			return audioFunctions == null ? default : audioFunctions.GetValue<T>(source, id);
		}

		public override void SetValue<T>(int id, T value, bool isOptional = false)
		{
			audioFunctions?.SetValue(source, id, value, isOptional);
		}
	}
}