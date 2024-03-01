using UnityEngine;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Animations.AnimationEvents
{
	public class AnimationEventBehaviour : ScriptableObject
	{
		[SerializeReference, Polymorphic]
		private IAnimationEvent _event;

		public void HandleEvent(Object target, Animator animator, AnimationEvent evnt)
		{
			_event?.Invoke(target, new AnimationEventInfo(animator, evnt));
		}
	}
}
