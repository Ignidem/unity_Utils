using UnityEngine;

namespace UnityUtils.Animations.AnimationEvents
{
	public struct InvokeEvent : IAnimationEvent
	{
		[SerializeField] private string subEventName;

		public readonly void Invoke(Object target, IAnimationEventInfo info)
		{
			if (info.State is IAnimationEventListener listener)
				listener.OnAnimationEvent(subEventName, info);
		}
	}
}
