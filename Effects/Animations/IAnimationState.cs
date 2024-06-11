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
		float Time { get; }
		float Weight { get; }
		float NormalizedTime { get; }
		float RemainingTime => Length - Time;

		public IAnimationEventBroadcaster EventBroadcaster { get; set; }

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
