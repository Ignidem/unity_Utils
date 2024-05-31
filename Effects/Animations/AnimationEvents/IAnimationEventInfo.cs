using UnityEngine;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Animations.AnimationEvents
{
	public interface IAnimationEventInfo
	{
		Transform Target { get; }
		float Time { get; }
		IAnimationState State { get; }
		AnimationClip Clip { get; }
	}

	public readonly struct AnimationEventInfo : IAnimationEventInfo
	{
		public Transform Target { get; }
		public readonly float Time => evnt.time;
		public readonly AnimationClip Clip => evnt.animatorClipInfo.clip;
		public IAnimationState State { get; }
		private readonly AnimationEvent evnt;

		public AnimationEventInfo(Animator animator, AnimationEvent evnt)
		{
			this.evnt = evnt;
			State = null;
			Target = animator.transform;
		}
	}
}
