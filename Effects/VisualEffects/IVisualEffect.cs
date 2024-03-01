using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IVisualEffect : IDisposable
	{
		bool IsPlaying { get; }
		Transform Root { get; }
	}
}
