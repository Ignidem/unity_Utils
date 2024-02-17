using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUutils.Animations
{

	public static class AnimatorEx
	{
		public static readonly int Empty = Animator.StringToHash(nameof(Empty));
		public static readonly int Exit = Animator.StringToHash(nameof(Exit));

		public static AnimatorStateInfo TriggerClip(this Animator anim, int id, int layer = 1, float blendTime = 0.1f)
		{
			if (blendTime > 0)
			{
				anim.CrossFade(id, blendTime, layer);
			}
			else
			{
				anim.Play(id, layer);
			}

			return anim.GetCurrentAnimatorStateInfo(layer);
		}

		public static AnimatorStateInfo ToggleClip(this Animator anim, int id, bool toggle, int layer = 1, float blendTime = 0.1f)
		{
			if (toggle)
				return anim.TriggerClip(id, layer, blendTime);

			AnimatorStateInfo activeState = anim.GetCurrentAnimatorStateInfo(layer);
			if (activeState.shortNameHash != id) 
				return activeState;

			if (anim.HasState(layer, Exit))
				return anim.TriggerClip(Exit, layer, blendTime);

			if (anim.HasState(layer, Empty))
				return anim.TriggerClip(Empty, layer, blendTime);

			return activeState;
		}
	
		public static AnimatorOverrideController GetOverrideController(this Animator anim)
		{
			if (anim.runtimeAnimatorController is AnimatorOverrideController overide)
				return overide;

			return new AnimatorOverrideController(anim.runtimeAnimatorController);
		}

		public static Task ToTask(this AnimatorStateInfo animState)
		{
			float norm = Mathf.Min(1, animState.normalizedTime);
			float time = animState.length * norm * animState.speed * animState.speedMultiplier;
			int ml = (int)(time * 1000);
			return Task.Delay(ml);
		}
		public static TaskAwaiter GetAwaiter(this AnimatorStateInfo animState)
		{
			return animState.ToTask().GetAwaiter();
		}
	}
}
