using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable]
	public struct Button3DAnimation : IButtonAnimations
	{
		[SerializeField] private Graphic front;
		[SerializeField] private Graphic back;
		[SerializeField] private Vector2 offset;
		[SerializeField] private Color color;
		[SerializeField] private float disabledMult;
		[SerializeField] private float highlightMult;

		public readonly void DoStateTransition(ButtonState state, bool animate)
		{
			bool isPressed = state is ButtonState.Selected or ButtonState.GroupSelected 
				or ButtonState.Pressed or ButtonState.Highlighted;

			if (back) back.enabled = !isPressed;
			if (!front) return;

			front.rectTransform.localPosition = isPressed ? Vector3.zero : offset * back.rectTransform.rect.size;

			front.color = state switch
			{
				ButtonState.Disabled => color * disabledMult,
				ButtonState.Highlighted => color * highlightMult,
				_ => color
			};
		}

		public readonly void SetSprite(Sprite sprite)
		{
			SetSprite(front, sprite);
			SetSprite(back, sprite);
		}

		private readonly void SetSprite(Graphic graphic, Sprite sprite)
		{
			switch (graphic)
			{
				case Image fimg:
					fimg.sprite = sprite;
					break;
				case SVGImage fsvg:
					fsvg.sprite = sprite;
					break;
			}
		}
	}
}
