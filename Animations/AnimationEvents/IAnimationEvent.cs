namespace UnityUtils.Animations.AnimationEvents
{
	public interface IAnimationEvent
	{
		void Invoke(UnityEngine.Object target, IAnimationEventInfo info);
	}
}
