using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.UI.Selectable
{
	public class ImageFitter : AspectRatioFitter
	{
#if UNITY_EDITOR
		protected override void OnValidate()
		{
			UpdateImageRatio();
			base.OnValidate();
		}
#endif

		public Sprite GetImageSprite()
		{
			if (gameObject.TryGetComponent(out Image image))
				return image.sprite;

			if (gameObject.TryGetComponent(out SVGImage svg))
				return svg.sprite;

			return null;
		}

		public Vector2 GetSpriteSize()
		{
			Sprite sprite = GetImageSprite();
			return sprite ? sprite.rect.size : Vector2.zero;
		}

		public void UpdateImageRatio()
		{
			Vector2 size = GetSpriteSize();
			float ratio = size.y == 0 ? 0 : (size.x / size.y);
			aspectRatio = ratio;
		}
	}
}
