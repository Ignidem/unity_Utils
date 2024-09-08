using UnityEngine;
using UnityUtils.Common.Layout;

namespace UnityUtils.DynamicScrollers
{
	public struct GridLayoutContainer : ILayoutGroupContainer
	{
		public readonly Vector2 Spacing => layout ? layout.GetSpacing() : Vector2.zero;
		public readonly RectOffset Padding => new RectOffset();
		public readonly Vector2Int GridSize => !layout ? Vector2Int.one : 
			(Vector2Int)layout.GetFittingGridSize(layout.transform.childCount);

		[SerializeField] private WorldGridLayout layout;

		public readonly Vector2 GetContentSize(RectTransform.Axis axis, RectTransform rect)
		{
			Vector2 spacing = Spacing;
			Vector2 size = rect.rect.size;
			Vector2Int grid = GridSize;
			return axis switch
			{
				RectTransform.Axis.Horizontal => new Vector2(spacing.x * grid.x, size.y),
				RectTransform.Axis.Vertical => new Vector2(size.x, spacing.y * grid.y),
				_ => throw new System.ArgumentOutOfRangeException()
			};
		}
	}
}
