using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;

namespace UnityUtils.UI.ImageComponents
{
	public class SVG : IImageComponent
	{
		[SerializeField] private SVGImage image;

		public bool IsAlive => image;
		public RectTransform Transform => image.transform as RectTransform;
		public Sprite OverrideSprite
		{
			get => isOverriden ? image.sprite : null;
			set
			{
				if (!isOverriden && value)
				{
					baseSprite = image.sprite;
					isOverriden = true;
				}
				else if (!value)
				{
					isOverriden = false;
					value = baseSprite;
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

		private Task<Sprite> loadingSprite;

		public async Task Load(Task<Sprite> spriteTask)
		{
			loadingSprite = spriteTask;
			Sprite sprite = await spriteTask;

			//Component is destroyed or task was overriden
			if (!image || loadingSprite != spriteTask)
				return;

			loadingSprite = null;
			OverrideSprite = sprite;
		}
	}
}
