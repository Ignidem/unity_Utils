using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Animations.PlayableAnimator
{
	[Serializable]
	public class PlayableAnimationState : IAnimationState
	{
		[SerializeField]
		private AnimationClip clip;

		[field: SerializeField]
		public string StateName { get; private set; }
		public int Id { get; private set; }
		public float Length => clip.length;

		public bool IsPlaying { get; private set; }

		private AnimationClipPlayable clipPlayable;

		public PlayableAnimationState() { }

		public PlayableAnimationState(AnimationClip clip, PlayableGraph graph, Animator animator, string name = null)
		{
			this.clip = clip;
			StateName = name ?? clip.name;
			Id = Animator.StringToHash(StateName);
			AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, StateName, animator);
			clipPlayable = AnimationClipPlayable.Create(graph, clip);
			output.SetSourcePlayable(clipPlayable);
		}

		public void UpdatePlayableData(PlayableGraph graph, Animator animator)
		{
			StateName ??= clip.name;
			Id = Animator.StringToHash(StateName);
			AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, StateName, animator);
			clipPlayable = AnimationClipPlayable.Create(graph, clip);
			output.SetSourcePlayable(clipPlayable);
		}

		public TaskAwaiter GetAwaiter()
		{
			if (!IsPlaying)
				return Task.CompletedTask.GetAwaiter();

			return OnStopAsync().GetAwaiter();
		}

		private async Task OnStopAsync()
		{
			while (IsPlaying)
				await Task.Yield();
		}

		public void Stop(float blendTime = 0.1F)
		{
			clipPlayable.Pause();
		}

		public void Play(float blendTime = 0.1F)
		{
			clipPlayable.Play();
		}
	}
}
