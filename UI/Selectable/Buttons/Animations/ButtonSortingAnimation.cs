using DG.Tweening;
using System;
using UnityEngine;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.UI.Selectable
{
	[Serializable]
	public class ButtonSortingAnimation : IButtonAnimations
	{
		[SerializeField] private Canvas target;
		[SerializeField] private float tweenDuration;
		[SerializeField] private EnumPair<ButtonState, int> sorting;

		public void DoStateTransition(ButtonState state, bool animate)
		{
			int pos = sorting[state];
			if (animate)
			{
				DOTween.To(() => target.sortingOrder, (int v) => target.sortingOrder = v, pos, tweenDuration);
			}
			else
			{
				target.sortingOrder = pos;
			}
		}
	}
}
