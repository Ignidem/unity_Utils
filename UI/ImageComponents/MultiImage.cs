using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.UI.ImageComponents
{
	[Serializable]
	public class MultiImage : IImageComponent
	{
		[SerializeReference, Polymorphic] private IImageComponent[] images;

		private bool HasSubComponents => images != null && images.Length > 0;
		public IImageComponent Main => HasSubComponents ? images[0] : null;
		public bool IsAlive => HasSubComponents && Main.IsAlive;
		public RectTransform Transform => Main?.Transform;
		public Material Material => Main?.Material;

		public bool Enabled 
		{
			get => Main.Enabled;
			set
			{
				for (int i = 0; i < images.Length; i++)
					images[i].Enabled = value;
			}
		}
		public Sprite OverrideSprite
		{
			get => Main.OverrideSprite;
			set
			{
				for (int i = 0; i < images.Length; i++)
					images[i].OverrideSprite = value;
			}
		}
		public Sprite Sprite
		{
			get => Main.Sprite;
			set
			{
				for (int i = 0; i < images.Length; i++)
					images[i].Sprite = value;
			}
		}
		public Color Color
		{
			get => Main.Color;
			set
			{
				for (int i = 0; i < images.Length; i++)
					images[i].Color = value;
			}
		}


		private Task<Sprite> loadingSprite; 
		
		public async Task Load(Task<Sprite> spriteTask)
		{
			loadingSprite = spriteTask;
			Sprite sprite = await spriteTask;

			//Component is destroyed or task was overriden
			if (!IsAlive || loadingSprite != spriteTask)
				return;

			loadingSprite = null;
			OverrideSprite = sprite;
		}
	}
}
