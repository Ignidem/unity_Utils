using UnityEngine;

namespace UnityUtils.Layouts.RectLayout
{
	[ExecuteInEditMode]
	public class RemainderLayoutElement : RectLayoutElement
	{
		protected override Rect GetVerticalRectLayout(Rect offset, Rect source)
		{
			return new Rect(offset.xMax, offset.yMax, source.width, source.height - offset.yMax);
		}

		protected override Rect GetHorizontalRectLayout(Rect offset, Rect source)
		{
			return new Rect(offset.xMax, offset.yMax, source.width - offset.xMax, source.height);
		}
	}
}