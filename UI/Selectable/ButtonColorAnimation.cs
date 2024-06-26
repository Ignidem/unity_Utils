using System.Collections;
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

		private Coroutine colorLerp;

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
				if (colorLerp != null)
					graphic.StopCoroutine(colorLerp);

				colorLerp = graphic.StartCoroutine(LerpColor(colors[state]));
			}
			else
			{
				graphic.color = colors[state];
			}
		}

		private IEnumerator LerpColor(Color color)
		{
			const float duration = 0.3f;
			Color current = graphic.color;
			for (float time = 0; time < duration; time += Time.deltaTime)
			{
				graphic.color = Color.Lerp(current, color, time / duration);
				yield return null;
			}

			this.colorLerp = null;
		}
	}
}
