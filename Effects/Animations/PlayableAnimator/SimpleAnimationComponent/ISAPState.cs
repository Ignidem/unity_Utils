namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public interface ISAPState : IState
	{
		bool IState.IsValid => IsStateValid();
		bool IsStateValid();
	}
}
