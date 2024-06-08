using UnityEngine;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Animations.AnimationEvents
{
	public class AnimationEventBehaviour : ScriptableObject
	{
		[SerializeReference, Polymorphic(true)]
		private IAnimationEvent _event;

		[SerializeReference, Polymorphic]
		private IAnimationEvent[] _events;

		private void OnValidate()
		{
			if (_event != null)
			{
				Debug.LogWarning("_event field is depricated, please move to _events. " + name);
			}
		}

		public void HandleEvent(Object target, Animator animator, AnimationEvent evnt)
		{
			AnimationEventInfo info = new AnimationEventInfo(animator, evnt);
			_event?.Invoke(target, info);

			if (_events == null)
				return;

			for (int i = 0; i < _events.Length; i++)
			{
				_events[i]?.Invoke(target, info);
			}
		}
	}
}
