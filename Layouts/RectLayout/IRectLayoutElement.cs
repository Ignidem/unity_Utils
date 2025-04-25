using UnityEngine;

namespace UnityUtils.Layouts.RectLayout
{
	public interface IRectLayoutElement
	{
		bool IsEnabled { get; }
		RectTransform Transform { get; }
		Vector2Int OffsetDirection { get; }
		Rect GetRectLayout(Rect offset, Rect source, bool animate);
	}
}
