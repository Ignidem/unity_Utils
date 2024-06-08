using UnityEngine;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Animations.AnimationEvents
{
	public interface IAnimationEvent
	{
		void Invoke(Object target, IAnimationEventInfo info);
	}
}
