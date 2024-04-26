using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.Animations.StateListener
{
	public interface IAnimationState
	{
		string StateName { get; }
		int Id { get; }
		bool IsPlaying { get; }
		float Length { get; }
		int Layer { get; }

		void Play(float blendTime = 0.1f);
		void Stop(float blendTime = 0.1f);
		TaskAwaiter GetAwaiter();
		async Task GetTask()
		{
			await this;
		}
	}

	public readonly struct AnimationState : IAnimationState
	{
		public string StateName { get; }
		public int Id { get; }
		public readonly bool IsPlaying => state.enabled;
		public float Length => state.length;
		public int Layer => state.layer;

		private readonly Animator animator;
		private readonly UnityEngine.AnimationState state;

		public AnimationState(Animator animator, UnityEngine.AnimationState state)
		{
			this.animator = animator;
			this.state = state;
			StateName = state.name;
			Id = Animator.StringToHash(StateName);
			this.state = state;
		}

		public TaskAwaiter GetAwaiter()
		{
			if (!IsPlaying)
				return Task.CompletedTask.GetAwaiter();
			int time = (int)((state.length - state.time) * 1000);
			return Task.Delay(time).GetAwaiter();
		}

		public void Play(float blendTime = 0.1F)
		{
			if (blendTime > 0)
			{
				animator.CrossFade(Id, blendTime, Layer);
			}
			else
			{
				animator.Play(Id, Layer);
			}
		}

		public void Stop(float blendTime = 0.1F)
		{
			throw new System.NotImplementedException();
		}
	}
}
