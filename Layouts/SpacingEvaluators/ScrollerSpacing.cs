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
}
