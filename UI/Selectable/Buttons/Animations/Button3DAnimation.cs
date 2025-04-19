using UnityEngine;
using UnityUtils.Serialization.Properties;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable]
	public class Button3DAnimation : ShadowedIconAnimation
	{
		[SerializeField] private Optional<Color> color;
		[SerializeField] private float disabledMult;
		[SerializeField] private float highlightMult;

		public override void DoStateTransition(ButtonState state, bool animate)
		{
			base.DoStateTransition(state, animate);
			if (!front || !color) return;

			front.color = state switch
			{
				ButtonState.Disabled => (Color)color * disabledMult,
				ButtonState.Highlighted => (Color)color * highlightMult,
				_ => color
			};
		}
	}
}
