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

	public struct CellSizeLayoutContainer : ILayoutGroupContainer
	{
		public readonly Vector2 Spacing => Vector2.zero;
		public readonly RectOffset Padding => new RectOffset();
		public readonly Vector2Int GridSize => new Vector2Int(1 ,1);

		[SerializeField] private DynamicScroller scroller;

		public readonly Vector2 GetContentSize(RectTransform.Axis axis, RectTransform transform)
		{
			Vector2 axisSizing = axis switch
			{ 
				RectTransform.Axis.Horizontal => new Vector2(1, 0),
				RectTransform.Axis.Vertical => new Vector2(0, 1),
				_ => Vector2.zero
			};

			Rect rect = transform.rect;
			Vector2 size = axis switch
			{
				RectTransform.Axis.Horizontal => new Vector2(0, rect.size.y),
				RectTransform.Axis.Vertical => new Vector2(rect.size.x, 0),
				_ => Vector2.zero
			};

			IScrollerCellData[] data = scroller.Data;
			for (int i = 0; i < data.Length; i++)
			{
				IScrollerCell cell = scroller.GetCellAt(i);
				Vector2 cellSize = cell.GetSize(rect, axis);
				size += cellSize * axisSizing;
				cell.Transform.sizeDelta = size;
			}

			return size;
		}
	}
}
