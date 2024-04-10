using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public static class PlayableAnimatorUtils
	{
		public static void Play(this Animator animator, Playable playable, PlayableGraph graph)
		{
			AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(graph, "AnimationClip", animator);
			playableOutput.SetSourcePlayable(playable, 0);
			graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			graph.Play();
		}
	}
}
