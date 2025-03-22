namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public interface IStateHandle : IState
	{
		bool IState.IsValid => IsStateValid();
		bool IsStateValid();
	}
}
