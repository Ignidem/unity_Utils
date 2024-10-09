using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.UI.ImageComponents
{
	public class Raster : IImageComponent
	{
		[SerializeField] private Image image;

		public RectTransform Transform => image.transform as RectTransform;
		public Sprite OverrideSprite
		{
			get => image.overrideSprite;
			set => image.overrideSprite = value;
		}
		public Sprite Sprite
		{
			get => image.sprite;
			set => image.sprite = value;
		}
		public Color Color
		{
			get => image.color;
			set => image.color = value;
		}
	}
}
