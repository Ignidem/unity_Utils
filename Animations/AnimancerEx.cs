using Animancer;
using Mono.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.Animations
{
	public static class AnimancerEx
	{
		public static AnimancerState Play(this AnimancerComponent anim, AnimationClip clip, int layer, float fadeDuration = 0.25f)
		{
			AnimancerLayer animLayer = anim.Layers[layer];
			return animLayer.Play(clip, fadeDuration);
		}

		public static async Task EndAsync(this AnimancerState state)
		{
			if (state == null)
				return;

			IEnumerator e = state;
			while (e.MoveNext())
				await Task.Yield();
		}

		public static TaskAwaiter GetAwaiter(this AnimancerState state)
		{
			if (state == null)
				return Task.CompletedTask.GetAwaiter();

			return state.EndAsync().GetAwaiter();
		}
	}
}
