using UnityEngine;
using UnityUtils.Animations.StateListener;
using Utils.Results;

namespace UnityUtils.Animations.AnimationEvents
{
	public interface IAnimationEventInfo
	{
		Result IsValid { get; }
		Transform Target { get; }
		float Time { get; }
		IAnimationState State { get; }
		AnimationClip Clip { get; }
	}

	public readonly struct AnimationEventInfo : IAnimationEventInfo
	{
		public static AnimationEventInfo MissingState(string stateName, string eventName, string source)
		{
			return new AnimationEventInfo($"Animation state {stateName} not found " +
				$"while searching for event {eventName} in {source}.");
		}

		public Result IsValid { get; }

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
			IsValid = evnt != null ? true : ("Event Not Found!\n" + new System.Diagnostics.StackTrace().ToString()[..1000]);
		}

		private AnimationEventInfo(Result error) : this()
		{
			IsValid = error;
		}
	}
}
