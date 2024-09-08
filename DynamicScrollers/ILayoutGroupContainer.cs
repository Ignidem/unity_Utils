using UnityEngine;

namespace UnityUtils.DynamicScrollers
{
	public interface ILayoutGroupContainer
	{
		Vector2 Spacing { get; }
		RectOffset Padding { get; }
		Vector2Int GridSize { get; }

		Vector2 GetContentSize(RectTransform.Axis scrollAxis, RectTransform rect);
	}
}
