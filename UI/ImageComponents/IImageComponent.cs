using UnityEngine;

namespace UnityUtils.UI.ImageComponents
{
	public interface IImageComponent
	{
		RectTransform Transform { get; }
		Sprite OverrideSprite { get; set; }
		Sprite Sprite { get; set; }
		Color Color { get; set; }
	}
}
