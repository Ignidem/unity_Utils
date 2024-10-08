using UnityEngine;

namespace UnityUtils.DynamicScrollers
{
	public partial class DynamicScroller
	{
		public delegate void CellClearedDelegate(IScrollerCell cell);
		public event CellClearedDelegate OnCellCleared;
		protected virtual void ClearCell(IScrollerCell cell)
		{
			cell.Clear();
			OnCellCleared?.Invoke(cell);
		}

		public delegate void CellInitializedDelegate(IScrollerCell cell);
		public event CellClearedDelegate OnCellInitialized;
		protected virtual void InitializeCell(IScrollerCell cell, int cellIndex, int dataIndex)
		{
			cell.DataIndex = dataIndex;
			cell.CellIndex = cellIndex;
			cell.SetData(_data[dataIndex]);
			cell.Transform.SetSiblingIndex(cellIndex);
			cells[cellIndex] = cell;

			if (contentComponents.Sizing == ContentComponents.SizingType.Additive)
			{
				Vector2 cellSize = cell.GetSize(content.rect, ScrollAxis);
				AddViewportSize(cellSize, cellIndex);
			}

			OnCellInitialized?.Invoke(cell);
		}
	}
}
