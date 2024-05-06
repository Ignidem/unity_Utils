using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.Animations.StateListener
{
	public delegate void AnimationEventListener(IAnimationState state, AnimationEvent evnt);

	public interface IAnimationState
	{
		string StateName { get; }
		int Id { get; }
		bool IsPlaying { get; }
		float Length { get; }
		int Layer { get; }
		float Time { get; }

		event AnimationEventListener OnEvent;

		void Play(float blendTime = 0.1f);
		void Stop(float blendTime = 0.1f);
		void Pause();
		void Resume();

		TaskAwaiter GetAwaiter();
		async Task GetTask()
		{
			await this;
		}
	}
}
