﻿using UnityEngine;
using UnityUtils.GameObjects.ObjectCaches.Caches;
using UnityUtils.Sounds;

namespace UnityUtils.Animations.AnimationEvents
{
	[System.Serializable]
	public struct SoundAnimationEvent : IAnimationEvent
	{
		private static AudioCache cache;
		private static AudioCache Cache
		{
			get
			{
				if (cache != null && cache.IsAlive)
					return cache;

				return cache = AudioCache.GetOrCreate(null);
			}
		}

		[SerializeField]
		private AudioClipCollection clips;

		public readonly void Invoke(Object target, IAnimationEventInfo info)
		{
			if (!clips)
				return;

			Transform transform = target switch
			{
				Transform tr => tr,
				Component comp => comp.transform,
				GameObject obj => obj.transform,
				_ => null
			};

			_ = clips.PlayRandom(transform, clip =>
			{
				clip.Volume = 1;
				clip.SpacialBlend = 1;
			});
		}
	}
}
