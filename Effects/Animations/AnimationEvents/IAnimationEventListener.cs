using UnityEngine;

namespace UnityUtils.Animations.AnimationEvents
{
	public interface IAnimationEventListener
	{
		void OnAnimationEvent(AnimationEvent evnt);
		void OnAnimationEvent(string eventName, IAnimationEventInfo evnt);
	}

	public interface IAnimationEventListenerHandler : IAnimationEventListener
	{
		Animator Animator { get; }
	}
}
