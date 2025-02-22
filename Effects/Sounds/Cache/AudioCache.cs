using UnityEngine;
using UnityEngine.Audio;

namespace UnityUtils.GameObjects.ObjectCaches.Caches
{
	public class AudioCache : BaseObjectCache<AudioResource, CachedAudio>, 
		ISyncObjectCache<AudioResource, CachedAudio>
	{
		public CachedAudio this[AudioResource key] => this.PopOrCreate(key);
		public static AudioCache GetOrCreate(Transform parent)
		{
			ObjectCacheController controller = ObjectCacheController.GetOrCreate(parent);
			return (AudioCache)controller.GetOrCreateCache(() => new AudioCache(controller.transform, false));
		}

		public AudioCache(Transform parent, bool withController) : base(parent, withController) { }

		public CachedAudio Create(AudioResource key)
		{
			GameObject go = new GameObject(key.name);
			AudioSource source = go.AddComponent<AudioSource>();
			source.resource = key;
			CachedAudio value = new CachedAudio(source);
			value.OnPop(this);
			return value;
		}
	}
}
