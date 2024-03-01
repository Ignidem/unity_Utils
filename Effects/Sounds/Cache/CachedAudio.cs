using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.Sounds;

namespace UnityUtils.GameObjects.ObjectCaches.Caches
{
	public class CachedAudio : ICacheableObject
	{
		public bool IsAlive => audio && audio.gameObject;
		public bool IsActive => IsAlive && audio.isPlaying;
		public Transform Transform => audio.transform;

		public float Volume
		{
			get => audio.volume;
			set => audio.volume = value;
		}
		public float SpacialBlend
		{
			get => audio.spatialBlend;
			set => audio.spatialBlend = value;
		}
		private readonly AudioSource audio;

		private AudioCache audioCache;

		public CachedAudio(AudioSource source)
		{
			this.audio = source;
		}

		public void Destroy()
		{
			Object.Destroy(audio.gameObject, audio.RemainingTime());
		}

		public void OnCached(IObjectCache cache)
		{
			Transform.gameObject.SetActive(false);
			if (cache is not AudioCache audioCache)
				return;

			this.audioCache = audioCache;
		}

		public void OnPop(IObjectCache cache) 
		{
			audioCache = cache as AudioCache;
			Transform.gameObject.SetActive(true);
		}

		public void Play(Transform source, bool cacheOnEnd)
		{
			if (source != null)
				Transform.SetParent(source);

			Play(cacheOnEnd);
		}

		public void Play(Vector3 position, bool cacheOnEnd)
		{
			Transform.position = position;
			Play(cacheOnEnd);
		}

		private void Play(bool cacheOnEnd)
		{
			Transform.gameObject.SetActive(true);
			audio.Play();

			if (cacheOnEnd)
				CacheOnEnd();
		}

		private async void CacheOnEnd()
		{
			while (audio.isPlaying)
			{
				await Task.Yield();

				if (DestroyIfCacheless()) return;
			}

			if (!DestroyIfCacheless())
				audioCache.Cache(audio.clip, this);
		}

		private bool DestroyIfCacheless()
		{
			if (audioCache == null || !audioCache.IsAlive)
			{
				Destroy();
				return true;
			}

			return false;
		}

		public void Release()
		{
			Destroy();
		}
	}
}
