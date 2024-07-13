using UnityEngine;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Animations.AnimationEvents
{
	public interface IAnimationEventInfo
	{
		bool IsValid { get; }
		Transform Target { get; }
		float Time { get; }
		IAnimationState State { get; }
		AnimationClip Clip { get; }
	}

	public readonly struct AnimationEventInfo : IAnimationEventInfo
	{
		public bool IsValid => evnt != null;
		public Transform Target { get; }
		public readonly float Time => evnt.time;
		public readonly AnimationClip Clip => evnt.animatorClipInfo.clip;
		public IAnimationState State { get; }
		private readonly AnimationEvent evnt;

		public AnimationEventInfo(Animator animator, AnimationEvent evnt, IAnimationState state)
		{
			this.evnt = evnt;
			State = state;
			Target = animator.transform;
		}
	}
}
