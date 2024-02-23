using UnityEngine;
using UnityUtils.GameObjects.ObjectCaches;
using UnityUtils.GameObjects.ObjectCaches.Caches;
using UnityUtils.Sounds;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable]
	public struct ButtonSounds : IButtonAnimations
	{

		private static AudioCache cache;
		private static AudioCache Cache
		{
			get
			{
				if (cache != null && cache.IsAlive)
					return cache;

				ObjectCacheController controller = ObjectCacheController.GetOrCreate(null);
				return cache = (AudioCache)controller.GetOrCreateCache(() => new AudioCache(controller.transform, false));
			}
		}

		[SerializeField]
		private Transform button;

		[SerializeField]
		private EnumPair<ButtonState, AudioClip> sources;

		public readonly void DoStateTransition(ButtonState state, bool animate)
		{
			AudioClip clip = sources[state];
			if (!clip)
				return;

			CachedAudio audio = Cache[clip];
			audio.Volume = 0.25f;
			audio.SpacialBlend = 0;
			audio.Play(button.position, true);
		}
	}
}
