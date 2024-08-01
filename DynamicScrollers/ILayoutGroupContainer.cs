using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Common.Layout;

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
		public readonly Vector2Int GridSize => layout ? (Vector2Int)layout.GridSize : Vector2Int.one;

		[SerializeField] private WorldGridLayout layout;

		public readonly Vector2 GetContentSize(RectTransform.Axis axis, RectTransform rect)
		{
			var space = Spacing * GridSize;
			return axis switch
			{
				RectTransform.Axis.Horizontal => new Vector2(space.x, rect.rect.size.y),
				RectTransform.Axis.Vertical => new Vector2(rect.rect.size.x, space.y),
				_ => throw new System.ArgumentOutOfRangeException()
			};
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
