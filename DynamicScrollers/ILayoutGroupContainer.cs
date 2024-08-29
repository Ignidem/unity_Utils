using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Common.Layout;
using Utilities.Numbers;

namespace UnityUtils.DynamicScrollers
{
	public interface ILayoutGroupContainer
	{
		Vector2 Spacing { get; }
		RectOffset Padding { get; }
		Vector2Int GridSize { get; }

		Vector2 GetContentSize(RectTransform.Axis scrollAxis, RectTransform rect);
	}

	public struct GridLayoutContainer : ILayoutGroupContainer
	{
		public readonly Vector2 Spacing => layout ? layout.GetSpacing() : Vector2.zero;
		public readonly RectOffset Padding => new RectOffset();
		public readonly Vector2Int GridSize => !layout ? Vector2Int.one : 
			(Vector2Int)layout.GetFittingGridSize(layout.transform.childCount);

		[SerializeField] private WorldGridLayout layout;
		[SerializeField] private float cellRatio;

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

		private readonly Vector2 HorizontalSize(Vector2 size, Vector2Int grid)
		{
			float singleCellHeight = size.y.SafeDivide(grid.y);
			return new Vector2(singleCellHeight * cellRatio * grid.x, size.y);
		}
		private readonly Vector2 VerticalSize(Vector2 size, Vector2Int grid)
		{
			//grid.x cell count occupy size.x width, therefore
			float singleCellWidth = size.x.SafeDivide(grid.x);
			float singleCellHeight = singleCellWidth.SafeDivide(cellRatio);
			return new Vector2(size.x, singleCellHeight * grid.y);
		}
	}
	public struct LayoutGroupContainer : ILayoutGroupContainer
	{
		public readonly Vector2 Spacing
		{
			get
			{
				if (!layout)
					return Vector2.zero;

				float spacing = layout.spacing;
				return layout switch
				{
					HorizontalLayoutGroup => new Vector2(spacing, 0),
					VerticalLayoutGroup => new Vector2(0, spacing),
					_ => Vector2.zero
				};
			}
		}

		public readonly RectOffset Padding => layout ? layout.padding : default;
		public readonly Vector2Int GridSize
		{
			get
			{
				return layout switch
				{
					HorizontalLayoutGroup => new Vector2Int(-1, 0),
					VerticalLayoutGroup => new Vector2Int(0, -1),
					_ => Vector2Int.zero
				};
			}
		}

		[SerializeField] private HorizontalOrVerticalLayoutGroup layout;

		public readonly Vector2 GetContentSize(RectTransform.Axis scrollAxis, RectTransform rect)
		{
			return rect.sizeDelta;
		}
	}
}
