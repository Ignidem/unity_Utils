using UnityEngine;

namespace UnityUtils.Animations.AnimationEvents
{
	public interface IEventSubstitute : IAnimationEvent
	{
		public bool IsName(string name);
	}

	public struct InvokeEvent : IEventSubstitute
	{
		[SerializeField]
		private string subEventName;

		public readonly void Invoke(Object target, IAnimationEventInfo info)
		{
			if (target is IAnimationEventListener listener)
				listener.OnAnimationEvent(subEventName, info);
		}

		public readonly bool IsName(string name) => name == subEventName;
	}

	public struct IgnoreEvent : IEventSubstitute
	{
		[SerializeField]
		private string ignoredEvent;

		public readonly void Invoke(Object target, IAnimationEventInfo info) { }

		public readonly bool IsName(string name) => ignoredEvent == name;
	}
}
