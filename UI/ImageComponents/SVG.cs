using Unity.VectorGraphics;
using UnityEngine;

namespace UnityUtils.UI.ImageComponents
{
	public class SVG : IImageComponent
	{
		[SerializeField] private SVGImage image;

		public RectTransform Transform => image.transform as RectTransform;
		public Sprite OverrideSprite
		{
			get => isOverriden ? image.sprite : null;
			set
			{
				if (!isOverriden)
				{
					baseSprite = image.sprite;
					isOverriden = true;
				}

				image.sprite = value;
			}
		}
		public Sprite Sprite
		{
			get => isOverriden ? baseSprite : image.sprite;
			set
			{
				if (isOverriden)
				{
					baseSprite = value;
				}
				else
				{
					image.sprite = value;
				}
			}
		}
		public Color Color
		{
			get => image.color; 
			set => image.color = value; 
		}

		private bool isOverriden;
		private Sprite baseSprite;
	}
}
