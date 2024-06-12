using System.Linq;
using UnityEngine;
using UnityUtils.Animations.StateListener;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Animations.AnimationEvents
{
	public class AnimationEventBehaviour : ScriptableObject
	{
		[SerializeReference, Polymorphic]
		private IAnimationEvent[] _events;

		public bool HasSubEvent(string name)
		{
			return _events.Any(_e => _e is IEventSubstitute substitute && substitute.IsName(name));
		}

		public void HandleEvent(Object target, Animator animator, AnimationEvent evnt)
		{
			AnimationClip clip = evnt.animatorClipInfo.clip;
			IAnimationState state = target is IAnimatorHandlerProvider provider ? provider.Handler[clip] : null;
			AnimationEventInfo info = new AnimationEventInfo(animator, evnt, state);

			if (_events == null)
				return;

			for (int i = 0; i < _events.Length; i++)
			{
				_events[i]?.Invoke(target, info);
			}
		}
	}
}
