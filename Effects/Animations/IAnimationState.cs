﻿using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.Animations.AnimationEvents;

namespace UnityUtils.Animations.StateListener
{
	public delegate void AnimationEventListener(string subEvent, IAnimationEventInfo evnt);

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

		event AnimationEventListener OnEvent;
		Task<bool> OnEventAsync(string eventName, float offset);

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
