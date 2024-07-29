using System;
using UnityEngine;
using UnityUtils.Common.Layout;

namespace UnityUtils.Layouts.SpacingEvaluators
{
	[Serializable]
	public class AutoCanvasSpacing : ISpacingEvaluator
	{
		[SerializeField]
		private RectTransform container;

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
