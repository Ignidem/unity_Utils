using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.UI.ImageComponents
{
	public class Raster : IImageComponent
	{
		[SerializeField] private Image image;

		public bool IsAlive => image;
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
