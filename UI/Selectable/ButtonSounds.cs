using UnityEngine;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable]
	public struct ButtonSounds : IButtonAnimations
	{
		[SerializeField]
		private EnumPair<ButtonState, AudioClip> clips;

		public readonly void DoStateTransition(ButtonState state, bool animate)
		{
			AudioClip clip = clips[state];
			if (!clip) return;

			AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.1f);
		}
	}
}
