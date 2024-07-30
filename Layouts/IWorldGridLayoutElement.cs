using UnityEngine;

namespace UnityUtils.Common.Layout
{
	public interface IWorldGridLayoutElement
	{
		Transform Transform { get; }
		void SetLayoutPosition(LayoutElementInfo info);
	}

	public readonly struct LayoutElementInfo
	{
		public readonly int siblingIndex;
		public readonly Vector3Int gridPosition;
		public readonly Vector3Int actualGridSize;
		public readonly Vector3Int maxGridSize;
		public readonly Vector3 localPosition;
		public readonly Vector3 spacing;

		public LayoutElementInfo(int i, Vector3Int gridPos, 
			Vector3Int actualGridSize, Vector3Int maxGridSize, 
			Vector3 pos, Vector3 spacing)
		{
			siblingIndex = i;
			gridPosition = gridPos;
			this.actualGridSize = actualGridSize;
			this.maxGridSize = maxGridSize;
			localPosition = pos;
			this.spacing = spacing;
		}
	}
}
