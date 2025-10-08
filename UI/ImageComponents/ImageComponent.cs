using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.UI.ImageComponents
{
	public class ImageComponent : MonoBehaviour, IImageComponent
	{
		[SerializeReference, Polymorphic]
		private IImageComponent component;

		public bool IsAlive => gameObject && this;

		public bool Enabled 
		{ 
			get => enabled;
			set 
			{
				enabled = value;
				component.Enabled = value;
			}
		}

		public RectTransform Transform => transform as RectTransform;
		public Material Material => component.Material;

		public Sprite OverrideSprite 
		{ 
			get => component.OverrideSprite; 
			set => component.OverrideSprite = value; 
		}
		public Sprite Sprite 
		{
			get => component.Sprite; 
			set => component.Sprite = value; 
		}
		public Color Color 
		{ 
			get => component.Color; 
			set => component.Color = value; 
		}
		
		public Task Load(Task<Sprite> spriteTask)
		{
			return component.Load(spriteTask);
		}
	}
}
