using DG.Tweening;
using System;
using UnityEngine;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable, Obsolete]
	public class ScaleButtonAnimation : ButtonScaleAnimation { }

	[System.Serializable]
	public class ButtonScaleAnimation : IButtonAnimations
	{
		[SerializeField] private Transform target;
		[SerializeField] private float tweenDuration;
		[SerializeField] private EnumPair<ButtonState, Vector3> scales;

		public ButtonScaleAnimation() { }
		public ButtonScaleAnimation(IButtonAnimations clone) 
		{
			if (clone is ButtonScaleAnimation scaler)
			{
				target = scaler.target;
				tweenDuration = scaler.tweenDuration;
				scales = scaler.scales;
			}
		}

		public void DoStateTransition(ButtonState state, bool animate)
		{
			Vector3 scale = scales[state];
			if (animate)
			{
				target.DOScale(scale, tweenDuration);
			}
			else
			{
				target.localScale = scale;
			}
		}
	}
}
