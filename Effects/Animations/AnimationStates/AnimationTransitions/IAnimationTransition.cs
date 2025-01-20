using UnityUtils.Animations.StateListener;

namespace UnityUtils.Effects.Animations.AnimationStates
{
	public interface IAnimationTransition
	{
		bool Evaluate(IAnimationState state);
	}
}
