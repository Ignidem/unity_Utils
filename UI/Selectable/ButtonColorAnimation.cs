using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.UI.Selectable
{

	[System.Serializable]
	public struct ButtonColorAnimation : IButtonAnimations
	{
		[SerializeField]
		private Graphic graphic;

		[SerializeField]
		private EnumPair<ButtonState, Color> colors;

		private bool isGroupSelected;

		public void DoStateTransition(ButtonState state, bool animate)
		{
			if (!graphic)
				return;

			if (isGroupSelected && state is ButtonState.Normal or ButtonState.Selected)
				return;

			if (state == ButtonState.GroupSelected)
				isGroupSelected = true;

			if (state == ButtonState.GroupDeselected)
				isGroupSelected = false;

			if (animate)
			{
				graphic.DOColor(colors[state], 0.3f);
			}
			else
			{
				graphic.color = colors[state];
			}
		}
	}
}
