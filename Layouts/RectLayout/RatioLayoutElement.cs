using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Numbers;

namespace UnityUtils.Layouts.RectLayout
{
	[ExecuteInEditMode]
	public class RatioLayoutElement : RectLayoutElement
	{
		[SerializeField] public float ratio;
		[SerializeField] private bool useCurrentSize;

		private Rect Rect => ((RectTransform)transform).rect;

		protected override void OnSetEnabled(bool enabled)
		{
			gameObject.SetActive(enabled);
		}

		protected override Rect GetHorizontalRectLayout(Rect offset, Rect source)
		{
			return new Rect(offset.xMax, 0,
					ratio.GetWidthFromHeight(source.height), 
					useCurrentSize ? Rect.height : source.height
				);
		}
		protected override Rect GetVerticalRectLayout(Rect offset, Rect source)
		{
			return new Rect(0, offset.yMax,
					useCurrentSize ? Rect.width : source.width,
					ratio.GetHeightFromWidth(source.width)
				);
		}
	}
}
