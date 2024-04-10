using UnityEngine;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public interface State
	{
		bool enabled { get; set; }
		bool isValid { get; }
		float time { get; set; }
		float normalizedTime { get; set; }
		float speed { get; set; }
		string name { get; set; }
		float weight { get; set; }
		float length { get; }
		AnimationClip clip { get; }
		WrapMode wrapMode { get; set; }

	}

	public interface IPlayableAnimationState : IAnimationState//, State
	{
	
	}
}
