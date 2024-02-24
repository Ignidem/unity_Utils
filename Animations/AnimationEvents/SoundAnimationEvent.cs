using UnityEngine;
using UnityUtils.GameObjects.ObjectCaches.Caches;

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
		private AudioClip clip;

		public readonly void Invoke(Object target, IAnimationEventInfo info)
		{
			if (!clip)
				return;
			
			CachedAudio audio = Cache[clip];
			audio.Volume = 0.5f;
			audio.SpacialBlend = 1;
			audio.Play(info.Target, true);
		}
	}
}
