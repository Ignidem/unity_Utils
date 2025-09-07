using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.PropertyAttributes;
using UnityUtils.Serialization.Properties;
using UnityUtils.UI.Colors;

namespace UnityUtils.UI.ImageComponents
{
	public class DiscoloredImage : IImageComponent
	{
		[SerializeReference, Polymorphic] private IImageComponent image;
		[SerializeField] private Optional<float> saturation;
		[SerializeField] private Optional<float> darkness;
		
		public bool IsAlive => image.IsAlive;
		public RectTransform Transform => image.Transform;
		public Material Material => image.Material;

		public bool Enabled
		{
			get => image.Enabled;
			set => image.Enabled = value;
		}
		public Sprite OverrideSprite
		{
			get => image.OverrideSprite;
			set => image.OverrideSprite = value;
		}
		public Sprite Sprite
		{
			get => image.Sprite;
			set => image.Sprite = value;
		}
		public Color Color
		{
			get => image.Color;
			set => image.Color = Discolor(value);
		}

		private Color Discolor(Color value)
		{
			if (saturation)
				value = value.Saturate(saturation);

			if (darkness)
				value = value.Darken(darkness);
			
			return value;
		}

		public Task Load(Task<Sprite> spriteTask) => image.Load(spriteTask);
	}
}