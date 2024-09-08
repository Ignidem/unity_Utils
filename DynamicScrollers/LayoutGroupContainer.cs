using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.DynamicScrollers
{
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
