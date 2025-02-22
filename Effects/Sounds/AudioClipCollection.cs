using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityUtils.AddressableUtils;
using UnityUtils.GameObjects.ObjectCaches.Caches;
using Utilities.Extensions;

namespace UnityUtils.Sounds
{
	public class AudioClipCollection : ScriptableObject
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
		private AddressableReference<AudioResource>[] clipsAdrs;

		[SerializeField] private AudioMixerGroup mixerGroup;

		public async Task<AudioResource> GetRandom()
		{
			AddressableReference<AudioResource> adrs = clipsAdrs.RandomElement();
			IAddressable<AudioResource> result = await adrs.Load();
			return result.Target;
		}

		public async Task<CachedAudio> PlayRandom(Transform parent = null, Action<CachedAudio> beforePlay = null)
		{
			AudioResource clip = await GetRandom();
			CachedAudio audio = Cache[clip];
			if (parent != null) audio.SpacialBlend = 1;
			if (mixerGroup) audio.MixerGroup = mixerGroup;

			beforePlay?.Invoke(audio);
			audio.Play(parent, true);
			return audio;
		}
	}
}
