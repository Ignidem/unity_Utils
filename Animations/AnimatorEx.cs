using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUutils.Animations
{
	public static class AnimatorEx
	{
		public static readonly int Empty = Animator.StringToHash(nameof(Empty));
		public static readonly int Exit = Animator.StringToHash(nameof(Exit));

		public static void TriggerClip(this Animator anim, int id, int layer = 1, float blendTime = 0.1f)
		{
			if (blendTime > 0)
			{
				anim.CrossFade(id, blendTime, layer);
			}
			else
			{
				anim.Play(id, layer);
			}
		}

		public static void ToggleClip(this Animator anim, int id, bool toggle, int layer = 1, float blendTime = 0.1f)
		{
			if (toggle)
			{
				anim.TriggerClip(id, layer, blendTime);
				return;
			}

			AnimatorStateInfo activeState = anim.GetCurrentAnimatorStateInfo(layer);
			if (activeState.shortNameHash != id) return;

			if (anim.HasState(layer, Exit))
				anim.TriggerClip(Exit, layer, blendTime);

			else if (anim.HasState(layer, Empty))
				anim.TriggerClip(Empty, layer, blendTime);
		}
	
		public static TaskAwaiter GetAwaiter(this AnimatorStateInfo animState)
		{
			float norm = Mathf.Min(1, animState.normalizedTime);
			float time = animState.length * norm * animState.speed * animState.speedMultiplier;
			int ml = (int)(time * 1000);
			return Task.Delay(ml).GetAwaiter();
		}
	}
}
