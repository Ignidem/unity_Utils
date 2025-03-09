using System;
using UnityEngine;
using UnityUtils.Effects.VisualEffects.ParameterFunctions;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IAnimatorParameterFunctions : IParameterFunctions<Animator> { }

	[Serializable]
	public class AnimatorComponent : IVisualEffectComponent
	{
		[SerializeField] private Animator animator;

		[SerializeReference, InspectorName("Functions"), Polymorphic(true)]
		private IAnimatorParameterFunctions animatorFunctions;

		public string Name => animator.name;

		public void Play()
		{
			animator.enabled = true;
		}
		public void Stop()
		{
			//Should not disable animator in case of stop state.
		}

		public T GetValue<T>(int id)
		{
			return animatorFunctions != null ? animatorFunctions.GetValue<T>(animator, id) : default;
		}
		public void SetValue<T>(int id, T value, bool isOptional = false)
		{
			animatorFunctions?.SetValue<T>(animator, id, value, isOptional);
		}
	}
}
