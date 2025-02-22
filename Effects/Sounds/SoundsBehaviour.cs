using UnityEngine;

namespace UnityUtils.Sounds
{
	public class SoundsBehaviour : MonoBehaviour
	{
		[SerializeField] private AudioClipCollection clips;
		[SerializeField] private Transform target;

		public void PlayRandom()
		{
			if (!clips)
				return;

			_ = clips.PlayRandom(target, clip =>
			{
				if (target) clip.Transform.localPosition = Vector3.zero;
				else clip.Transform.position =  transform.position;
			});
		}
	}
}
