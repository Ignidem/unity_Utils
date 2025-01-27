using UnityEngine;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable]
	public class Button3DAnimation : ShadowedIconAnimation
	{
		[SerializeField] private Color color;
		[SerializeField] private float disabledMult;
		[SerializeField] private float highlightMult;

		public override void DoStateTransition(ButtonState state, bool animate)
		{
			base.DoStateTransition(state, animate);
			if (!front) return;

			front.color = state switch
			{
				ButtonState.Disabled => color * disabledMult,
				ButtonState.Highlighted => color * highlightMult,
				_ => color
			};
		}
	}
}
