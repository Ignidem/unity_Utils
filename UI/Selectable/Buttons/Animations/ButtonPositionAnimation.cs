using DG.Tweening;
using System;
using UnityEngine;
using UnityUtils.Storages.EnumPairLists;
using UnityUtils.Transforms;

namespace UnityUtils.UI.Selectable
{
	[Serializable]
	public class ButtonPositionAnimation : IButtonAnimations
	{
		[SerializeField] private Transform target;
		[SerializeField] private float tweenDuration;
		[SerializeField] private EnumPair<ButtonState, Vector3Info> positions;

		public void DoStateTransition(ButtonState state, bool animate)
		{
			Vector3Info pos = positions[state];
			if (animate)
			{
				if (pos.space == Space.World)
				{
					target.DOMove(pos, tweenDuration);
				}
				else
				{
					target.DOLocalMove(pos, tweenDuration);
				}
			}
			else
			{
				if (pos.space == Space.World)
				{
					target.position = pos;
				}
				else
				{
					target.localPosition = pos;
				}
			}
		}
	}
}
