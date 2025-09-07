using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.UI.ImageComponents
{
	public interface IImageComponent
	{
		public bool IsAlive { get; }
		public bool Enabled { get; set; }
		RectTransform Transform { get; }
		Material Material { get; }
		Sprite OverrideSprite { get; set; }
		Sprite Sprite { get; set; }
		Color Color { get; set; }

		public Task Load(Task<Sprite> spriteTask);
	}
}
