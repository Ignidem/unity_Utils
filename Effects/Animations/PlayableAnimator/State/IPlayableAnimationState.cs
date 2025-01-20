using UnityEngine;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public interface IState
	{
		bool Enabled { get; set; }
		bool IsValid { get; }
		float Time { get; set; }
		float NormalizedTime { get; set; }
		float Speed { get; set; }
		string Name { get; set; }
		float Weight { get; set; }
		float Length { get; }
		AnimationClip Clip { get; }
		WrapMode WrapMode { get; set; }

	}

	public interface IPlayableAnimationState : IAnimationState//, State
	{
	
	}
}
