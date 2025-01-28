using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable]
	public class ButtonColorAnimation : IButtonAnimations
	{
		[SerializeField] private Graphic graphic;
		[SerializeField] private bool useTint;
		[SerializeField] private float animationDuration = 0.3f;

		[SerializeField]
		private EnumPair<ButtonState, Color> colors;

		private bool isGroupSelected;

		private Coroutine colorLerp;

		public void DoStateTransition(ButtonState state, bool animate)
		{
			if (isGroupSelected && state is ButtonState.Normal or ButtonState.Selected)
				return;

			if (state == ButtonState.GroupSelected)
				isGroupSelected = true;

			if (state == ButtonState.GroupDeselected)
				isGroupSelected = false;

			Color targetColor = colors[state];

			if (useTint)
			{
				graphic.CrossFadeColor(targetColor, animate ? animationDuration : 0, true, true);
				return;
			}

			if (animate && graphic.isActiveAndEnabled)
			{
				if (colorLerp != null)
					graphic.StopCoroutine(colorLerp);

				colorLerp = graphic.StartCoroutine(LerpColor(targetColor));
			}
			else
			{
				graphic.color = targetColor;
			}
		}

		private IEnumerator LerpColor(Color color)
		{
			Color current = graphic.color;
			for (float time = 0; time < animationDuration; time += Time.deltaTime)
			{
				Color lerp = Color.Lerp(current, color, time / animationDuration);
				graphic.color = lerp;
				yield return null;
			}

			this.colorLerp = null;
		}
	}
}
