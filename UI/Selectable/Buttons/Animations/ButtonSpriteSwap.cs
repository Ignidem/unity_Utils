using UnityEngine;
using UnityUtils.PropertyAttributes;
using UnityUtils.Storages.EnumPairLists;
using UnityUtils.UI.ImageComponents;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable]
	public class ButtonSpriteSwap : IButtonAnimations
	{
		[SerializeReference, Polymorphic]
		private IImageComponent image;

		[SerializeField]
		private EnumPair<ButtonState, Sprite> sprites; 
		
		public void DoStateTransition(ButtonState state, bool animate)
		{
			if (image == null || !image.IsAlive)
				return;

			Sprite sprite = sprites[state];
			if (sprite == null)
				return;

			image.OverrideSprite = sprite;
		}
	}
}
