using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Effects.Animations.Mechanim
{
	public static class AnimatorEx
	{
		public static void Play(this Animator animator, int id, int layer, float blendTime)
		{
			if (blendTime > 0)
			{
				animator.CrossFade(id, blendTime, layer);
			}
			else
			{
				animator.Play(id, layer);
			}
		}

		public static IAnimationState[] GetStates<T>(this Animator animator)
			where T : struct, IPlayable
		{
			PlayableGraph graph = animator.playableGraph;
			Playable controller = animator.GetAnimatorPlayable();
			Playable layers = controller.GetInputs().FirstOrDefault(
				input => input.GetPlayableType() == typeof(AnimationLayerMixerPlayable)
			);
			int layerCount = layers.GetInputCount();
			List<IAnimationState> states = new List<IAnimationState>();

			for (int l = 0; l < layerCount; l++)
			{
				var layer = layers.GetInput(l);
				foreach (var clip in layer.GetInputs<AnimationClipPlayable>(true))
				{
					states.Add(PlayableState.Create(graph, clip, l));
				}
			}

			return states.ToArray();
		}

		public static Playable GetAnimatorPlayable(this Animator animator)
		{
			PlayableGraph graph = animator.playableGraph;
			int count = graph.GetPlayableCount();
			for (int i = 0; i < count; i++)
			{
				Playable playable = graph.GetRootPlayable(i);
				if (playable.IsValid() && playable.GetPlayableType() == typeof(AnimatorControllerPlayable))
					return playable;
			}

			return Playable.Null;
		}
		
		public static void GetOutputs(Playable playable)
		{
			int count = playable.GetInputCount();
			for (int i = 0; i < count; i++)
			{
				Playable ouptut = playable.GetInput(i);
				Type type = ouptut.GetPlayableType();
			}
		}

#if UNITY_EDITOR
		public static UnityEditor.Animations.AnimatorController GetController(this Animator animator)
		{
			if (animator == null)
			{
				throw new ArgumentNullException(nameof(animator));
			}

			RuntimeAnimatorController rootController = animator.runtimeAnimatorController;
			while (rootController is AnimatorOverrideController overide)
			{
				rootController = overide.runtimeAnimatorController;
			}

			string assetPath = UnityEditor.AssetDatabase.GetAssetPath(rootController);
			return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(assetPath);
		}

		public static AnimationClip GetStateClip(this Animator animator, int layer, int namehash)
		{
			if (animator == null)
			{
				throw new ArgumentNullException(nameof(animator));
			}

			MethodInfo GetOverrideClip = typeof(AnimatorOverrideController).GetMethod(nameof(GetOverrideClip), 
				BindingFlags.NonPublic | BindingFlags.Instance);

			RuntimeAnimatorController GetRoot(RuntimeAnimatorController controller, out AnimationClip originalClip)
			{
				if (controller is not AnimatorOverrideController overide)
				{
					string assetPath = UnityEditor.AssetDatabase.GetAssetPath(controller);
					var root = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(assetPath);
					var states = root.layers[layer].stateMachine.states;
					var state = states.FirstOrDefault(s => s.state.nameHash == namehash);
					originalClip = state.state.motion as AnimationClip;
					return controller;
				}

				//is not root;
				var rootController = GetRoot(overide.runtimeAnimatorController, out originalClip);
				originalClip = (AnimationClip)GetOverrideClip.Invoke(overide, new object[] { originalClip });
				return rootController;
			}

			var root = GetRoot(animator.runtimeAnimatorController, out AnimationClip clip);
			return clip;
		}
#endif
	}
}
