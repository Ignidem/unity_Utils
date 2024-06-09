using System.Linq;
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

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_event != null)
			{
				Debug.LogWarning("_event field is depricated, please move to _events. " + name);
			}
		}
		public bool HasSubEvent(string name)
		{
			return _events.Any(_e => _e is IEventSubstitute substitute && substitute.IsName(name));
		}
#endif

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
