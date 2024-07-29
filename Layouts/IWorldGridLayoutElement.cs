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
		public readonly Vector3 localPosition;
		public readonly Vector3 spacing;

		public LayoutElementInfo(int i, Vector3Int gridPos, Vector3 pos, Vector3 spacing) : this()
		{
			siblingIndex = i;
			gridPosition = gridPos;
			localPosition = pos;
			this.spacing = spacing;
		}
	}
}
