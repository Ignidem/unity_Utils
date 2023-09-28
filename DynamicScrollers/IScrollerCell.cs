using System;
using UnityEngine;

namespace UnityUtils.DynamicScrollers
{
	public interface IScrollerCellData
	{
		Type CellType { get; }
	}

	public interface IScrollerCell
	{
		Type CellType { get; }
		int CellIndex { get; set; }
		int DataIndex { get; set; }
		Transform Transform { get; }

		Vector2 GetSize(DynamicScroller scroller);

		bool SetData(IScrollerCellData data);
		void Refresh();
		void Clear();
	}
}