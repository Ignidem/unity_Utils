using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityUtils.Common.Layout;
using Utilities.Numbers;

namespace UnityUtils.Layouts.SpacingEvaluators
{
	[Serializable]
	public class ScrollerSpacing : ISpacingEvaluator
	{
		[SerializeField] private RectTransform container;
		[SerializeField] private Axis axis;
		[SerializeField] private float ratio;

		public Vector3 GetSpacing(int count, WorldGridLayout layout)
		{
			Vector3Int grid = layout.GetFittingGridSize(count);
			Vector2 size = container.rect.size;

			return axis switch
			{
				Axis.X => HorizontalSize(size, grid),
				Axis.Y => VerticalSize(size, grid),
				Axis.Z => throw new NotImplementedException(),
				_ => throw new NotImplementedException(),
			};
		}

		private Vector2 HorizontalSize(Vector2 size, Vector3Int grid)
		{
			float singleCellHeight = size.y.SafeDivide(grid.y);
			return new Vector2(singleCellHeight * ratio, singleCellHeight);
		}
		private Vector2 VerticalSize(Vector2 size, Vector3Int grid)
		{
			//grid.x cell count occupy size.x width, therefore
			float singleCellWidth = size.x.SafeDivide(grid.x);
			float singleCellHeight = singleCellWidth.SafeDivide(ratio);
			return new Vector2(singleCellWidth, singleCellHeight);
		}
	}

	[Serializable]
	public class AutoCanvasSpacing : ISpacingEvaluator
	{
		[SerializeField] private RectTransform container;

		public Vector3 GetSpacing(int count, WorldGridLayout layout)
		{
			Vector3Int grid = layout.GridSize;
			Vector2 size = container.rect.size;
			int xCount = Math.Min(count, grid.x);
			float xSpacing = size.x / xCount;

			int yCount = (int)Math.Ceiling((float)count / grid.x);
			yCount = Math.Min(yCount, grid.y);
			float ySpacing = size.y / yCount;

			int overflow = count - layout.MaxElements;
			if (overflow > 0)
			{
				if (layout.overflowX)
				{
					xCount += overflow;
				}
				else if (layout.overflowY)
				{
					yCount += (int)Math.Ceiling((float)overflow / grid.x);
				}
			}

			float x = Mathf.Lerp(xSpacing, 0, ((float)xCount - grid.x) / grid.x);
			float y = Mathf.Lerp(ySpacing, 0, ((float)yCount - grid.y) / grid.y);
			return new Vector3(x, y, 0);
		}
	}
}
