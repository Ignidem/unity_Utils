using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IVisualEffect : IVisualComponent
	{
		bool IsPlaying { get; }
		Transform Root { get; }

		T GetValue<T>(string component, int id);
		void SetValue<T>(string component, int id, T value);
		void SetAll<T>(int id, T value);

		void OnEnd(Action action);
	}
}
