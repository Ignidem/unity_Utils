using UnityEngine;

namespace UnityUtils.Animations.StateListener
{
	public interface IAnimatorHandlerProvider
	{
		IAnimatorHandler Handler { get; } 
	}

	public interface IAnimatorHandler
	{
		void RegisterState(IAnimationState state);

		void OnStateEnter(IAnimationState state);
		void OnStateExit(IAnimationState state);

		IAnimationState Play(string name, int layer = 1, float blendTime = 0.1f)
		{
			return SwitchState(Animator.StringToHash(name), layer, blendTime);
		}
		IAnimationState SwitchState(int id, int layer = 1, float blendTime = 0.1f);
	}
}
