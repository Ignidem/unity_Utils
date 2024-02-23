using UnityEngine;

namespace UnityUtils.Sounds
{
	public static class AudioClipEx
	{
		public static AudioSource Play(this AudioClip clip, Vector3 worldPosition, System.Action<AudioSource> beforePlay = null)
		{
			GameObject gameObject = new GameObject(clip.name);
			gameObject.transform.position = worldPosition;
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = clip;
			beforePlay?.Invoke(source);
			source.Play();
			return source;
		}

		public static float ScaledLength(this AudioClip clip)
		{
			return clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale);
		}

		public static float RemainingTime(this AudioSource source)
		{
			if (!source.isPlaying)
				return 0;
			return (source.clip.length - source.time) * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale);
		}
	}
}
