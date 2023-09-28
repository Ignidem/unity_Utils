using System;
using UnityEngine;
using UnityEngine.UI;
using Axis = UnityEngine.RectTransform.Axis;


namespace UnityUtils.DynamicScrollers
{
	public partial class DynamicScroller : ScrollRect
	{
		public static readonly string[] serializedFields =
		{
			nameof(cells),
			nameof(contentComponents),
		};

		[SerializeField] private Cells cells;
		[SerializeField] private ContentComponents contentComponents;

		private IScrollerCellData[] _data;
		public IScrollerCellData[] Data
		{
			get => _data;
			set
			{
				if (_data == value) return;

				_data = value;
				ReloadCells();
			}
		}

		public Axis ScrollAxis => vertical ? Axis.Vertical : Axis.Horizontal;

		public void ReloadCells()
		{
			int cellIndex = 0;
			int dataIndex = 0;

			void SetCell(IScrollerCell cell)
			{
				cell.DataIndex = dataIndex;
				cell.CellIndex = cellIndex;
				cell.Transform.SetSiblingIndex(cellIndex);
				cells[cellIndex] = cell;
				cellIndex++;

				Vector2 cellSize = cell.GetSize(content.rect, ScrollAxis);
				AddContentSize(cellSize, cellIndex);
			}

			ResetContentSize();

			for (; dataIndex < _data.Length; dataIndex++)
			{
				IScrollerCellData data = _data[dataIndex];
				IScrollerCell currentCell = cells[cellIndex];
				if (currentCell == null)
				{
					if (cells.TryRecycleOrCreate(data, content, out IScrollerCell cell))
						SetCell(cell);
				}
				else if (currentCell.CellType != data.CellType)
				{
					cells.RecycleAt(dataIndex);
					if (cells.TryRecycleOrCreate(data, content, out IScrollerCell cell))
						SetCell(cell);
				}
				else
				{
					currentCell.Clear();
					currentCell.SetData(data);
					SetCell(currentCell);
				}
			}

			for (; cellIndex < cells.Count; cellIndex++)
			{
				cells.RecycleAt(cellIndex);
			}
		}

		private void ResetContentSize()
		{
			content.sizeDelta = Vector2.zero;
			Axis scrollAxis = ScrollAxis;
			content.sizeDelta = contentComponents.StartPadding(scrollAxis) + contentComponents.EndPadding(scrollAxis);
		}

		private void AddContentSize(Vector2 cellSize, int cellIndex)
		{
			float padding = cellIndex == 0 ? 0 : contentComponents.Spacing;
			switch (ScrollAxis)
			{
				case Axis.Horizontal:
					content.sizeDelta += new Vector2(cellSize.x + padding, 0);
					break;
				case Axis.Vertical:
					content.sizeDelta += new Vector2(0, cellSize.y + padding);
					break;
			}
		}
	}
}
