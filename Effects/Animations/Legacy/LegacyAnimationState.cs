using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.Animations.StateListener;

namespace Utils.Effects.Animations.Legacy
{
	public readonly struct LegacyAnimationState : IAnimationState
	{
		private readonly Animation controller;
		private readonly UnityEngine.AnimationState state;

		public readonly string StateName => state.name;
		public readonly int Id => Animator.StringToHash(StateName);
		public readonly bool IsPlaying => controller.IsPlaying(StateName);
		public readonly float Length => state.length;
		public readonly int Layer => state.layer;

		public LegacyAnimationState(Animation controller, UnityEngine.AnimationState state)
		{
			this.controller = controller;
			this.state = state;
		}

		public readonly TaskAwaiter GetAwaiter()
		{
			return OnEndAsync().GetAwaiter();
		}

		private readonly async Task OnEndAsync()
		{
			while (IsPlaying)
				await Task.Yield();
		}

		public readonly void Play(float blendTime = 0.1F)
		{
			if (blendTime == 0)
			{
				controller.Play(StateName);
			}
			else
			{
				controller.Blend(StateName, 1, blendTime);
			}
		}

		public readonly void Stop(float blendTime = 0.1F)
		{
			if (blendTime == 0)
			{
				controller.Stop(StateName);
			}
			else
			{
				controller.Blend(StateName, 0, blendTime);
			}
		}
	}
}
