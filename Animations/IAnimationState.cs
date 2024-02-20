using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityUtils.Animations.StateListener
{
	public interface IAnimationState
	{
		string StateName { get; }
		int Id { get; }
		bool IsPlaying { get; }
		float Length { get; }

		void Play(float blendTime = 0.1f);
		void Stop(float blendTime = 0.1f);
		TaskAwaiter GetAwaiter();
	}
}
