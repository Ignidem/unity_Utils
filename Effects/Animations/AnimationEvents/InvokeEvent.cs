using UnityEngine;

namespace UnityUtils.Animations.AnimationEvents
{
	public interface IEventSubstitute : IAnimationEvent
	{
#if UNITY_EDITOR
		public bool IsName(string name);
#endif
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

#if UNITY_EDITOR
		public readonly bool IsName(string name) => name == subEventName;
#endif
	}

	public struct IgnoreEvent : IEventSubstitute
	{
		[SerializeField]
		private string ignoredEvent;

		public readonly void Invoke(Object target, IAnimationEventInfo info) { }

#if UNITY_EDITOR
		public readonly bool IsName(string name) => ignoredEvent == name;
#endif
	}
}
