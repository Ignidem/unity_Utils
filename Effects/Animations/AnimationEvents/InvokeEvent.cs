using UnityEngine;

namespace UnityUtils.Animations.AnimationEvents
{
	public struct InvokeEvent : IAnimationEvent
	{
		[SerializeField]
		private string subEventName;

		public readonly void Invoke(Object target, IAnimationEventInfo info)
		{
			if (target is IAnimationEventListener listener)
				listener.OnAnimationEvent(subEventName, info);
		}

#if UNITY_EDITOR
		public bool IsName(string name) => name == subEventName;
#endif
	}
}
