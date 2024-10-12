using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.UI.ImageComponents
{
	public interface IImageComponent
	{
		RectTransform Transform { get; }
		Sprite OverrideSprite { get; set; }
		Sprite Sprite { get; set; }
		Color Color { get; set; }

		public Task Load(Task<Sprite> spriteTask);
	}
}
