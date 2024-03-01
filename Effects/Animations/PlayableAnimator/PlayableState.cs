using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Effects.Animations.Mechanim
{
	public class PlayableState : IAnimationState, IPlayableBehaviour
	{
		public static PlayableState Create(PlayableGraph graph, Playable playable, int layer)
		{
			PlayableState state = graph.ConnectBehaviour<Playable, PlayableState>(playable);
			state.SetPlayable(playable);
			state.Layer = layer;
			return state;
		}

		public string StateName { get; private set; }
		public int Id { get; private set; }
		public bool IsPlaying => playable.GetPlayState() == PlayState.Playing;
		public float Length { get; private set; }
		public int Layer { get; private set; }
		public Type PlayableType { get; private set; }

		private float initialWeight;

		private Playable playable;

		private async Task EndAsync()
		{
			while (IsPlaying)
				await Task.Yield();
		}
		public TaskAwaiter GetAwaiter()
		{
			if (!IsPlaying)
				return Task.CompletedTask.GetAwaiter();

			return EndAsync().GetAwaiter();
		}

		public async void Play(float blendTime = 0.1F)
		{
			await Blend(blendTime, initialWeight);
			playable.Play();
		}
		public async void Stop(float blendTime = 0.1F)
		{
			await Blend(blendTime, 0);
			playable.Pause();
			playable.SetTime(0);
		}

		private async Task Blend(float blendTime, float targetWeight)
		{
			if (!IsPlaying || !playable.CanSetWeights() || blendTime <= 0)
				return;

			float time = 0;
			double lastTime = playable.GetTime();
			float initWeight = playable.GetInputWeight(Id);
			while (time < blendTime)
			{
				double currentTime = playable.GetTime();
				float delta = (float)Math.Abs(currentTime - lastTime);
				time += delta;
				float weight = Mathf.Lerp(initWeight, targetWeight, time / blendTime);
				playable.SetInputWeight(Id, weight);
				lastTime = currentTime;
				await Task.Yield();
			}
		}

		private void SetPlayable(Playable playable)
		{
			this.playable = playable;
			if (!playable.IsValid())
				return;
			Length = (float)playable.GetDuration();
			initialWeight = 1;// playable.GetOutput(0).GetInputWeight(Id);
			PlayableType = playable.GetPlayableType();
		}

		public void PrepareFrame(Playable playable, FrameData info) 
		{
			Debug.Log(nameof(PrepareFrame) + " " + playable.GetPlayableType().Name);
		}

		public void ProcessFrame(Playable playable, FrameData info, object playerData) 
		{
			Debug.Log(nameof(ProcessFrame) + " " + playable.GetPlayableType().Name);
		}

		public void OnBehaviourPause(Playable playable, FrameData info)
		{
			Debug.Log(nameof(OnBehaviourPause) + " " + playable.GetPlayableType().Name);
		}

		public void OnBehaviourPlay(Playable playable, FrameData info) 
		{
			Debug.Log(nameof(OnBehaviourPlay) + " " + playable.GetPlayableType().Name);
		}

		public void OnGraphStart(Playable playable) 
		{
			Debug.Log(nameof(OnGraphStart) + " " + playable.GetPlayableType().Name);
		}

		public void OnGraphStop(Playable playable) 
		{
			Debug.Log(nameof(OnGraphStop) + " " + playable.GetPlayableType().Name);
		}

		public void OnPlayableCreate(Playable playable) 
		{
			Debug.Log(nameof(OnPlayableCreate) + " " + playable.GetPlayableType().Name);
		}

		public void OnPlayableDestroy(Playable playable) 
		{
			Debug.Log(nameof(OnPlayableDestroy) + " " + playable.GetPlayableType().Name);
		}
	}
}
