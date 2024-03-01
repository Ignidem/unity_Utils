using System;
using System.Threading.Tasks;
using UnityEngine;
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
		private AddressableReference<AudioClip>[] clipsAdrs;

		public async Task<AudioClip> GetRandom()
		{
			AddressableReference<AudioClip> adrs = clipsAdrs.RandomElement();
			IAddressable<AudioClip> result = await adrs.Load();
			return result.Target;
		}

		public async Task<CachedAudio> PlayRandom(Transform parent = null, Action<CachedAudio> beforePlay = null)
		{
			AudioClip clip = await GetRandom();
			CachedAudio audio = Cache[clip];
			if (parent != null)
				audio.SpacialBlend = 1;

			beforePlay?.Invoke(audio);
			audio.Play(parent, true);
			return audio;
		}
	}
}
