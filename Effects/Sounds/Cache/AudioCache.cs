using UnityEngine;

namespace UnityUtils.GameObjects.ObjectCaches.Caches
{
	public class AudioCache : BaseObjectCache<AudioClip, CachedAudio>, 
		ISyncObjectCache<AudioClip, CachedAudio>
	{
		public CachedAudio this[AudioClip key] => this.PopOrCreate(key);
		public static AudioCache GetOrCreate(Transform parent)
		{
			ObjectCacheController controller = ObjectCacheController.GetOrCreate(parent);
			return (AudioCache)controller.GetOrCreateCache(() => new AudioCache(controller.transform, false));
		}

		public AudioCache(Transform parent, bool withController) : base(parent, withController) { }

		public CachedAudio Create(AudioClip key)
		{
			GameObject go = new GameObject(key.name);
			AudioSource source = go.AddComponent<AudioSource>();
			source.clip = key;
			CachedAudio value = new CachedAudio(source);
			value.OnPop(this);
			return value;
		}
	}
}
