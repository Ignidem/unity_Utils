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
			ResetViewportSize();

			int cellIndex = 0;
			for (int dataIndex = 0; dataIndex < _data.Length; dataIndex++)
			{
				cellIndex = ReloadAt(cellIndex, dataIndex);
			}

			for (; cellIndex < cells.Count; cellIndex++)
			{
				if (!cells.RecycleCellAt(cellIndex, out IScrollerCell cell)) continue;
				ClearCell(cell);
			}

			if (contentComponents.Sizing == ContentComponents.SizingType.OnReload)
			{
				Vector2 size = contentComponents.Layout.GetContentSize(ScrollAxis, viewport);
				SetContentSize(size);
			}
		}

		private int ReloadAt(int cellIndex, int dataIndex)
		{
			IScrollerCellData data = _data[dataIndex];
			IScrollerCell currentCell = cells[cellIndex];

			if (currentCell?.CellType == data.CellType)
			{
				ClearCell(currentCell);
				currentCell.SetData(data);
				InitializeCell(currentCell, cellIndex, dataIndex);
				return cellIndex + 1;
			}

			if (currentCell != null && cells.RecycleCellAt(dataIndex, out IScrollerCell cell))
				ClearCell(cell);

			if (cells.TryRecycleOrCreate(data, out cell))
			{
				InitializeCell(cell, cellIndex, dataIndex);
				cellIndex++;
			}

			return cellIndex;
		}

		private void ResetViewportSize()
		{
			content.sizeDelta = Vector2.zero;
			Axis scrollAxis = ScrollAxis;
			content.sizeDelta = contentComponents.StartPadding(scrollAxis) + contentComponents.EndPadding(scrollAxis);
		}

		private void SetContentSize(Vector2 size)
		{
			content.sizeDelta = size;
		}
		private void AddViewportSize(Vector2 cellSize, int cellIndex)
		{
			Vector2 padding = cellIndex == 0 ? Vector2.zero : contentComponents.Spacing;
			switch (ScrollAxis)
			{
				case Axis.Horizontal:
					content.sizeDelta += new Vector2(cellSize.x + padding.x, 0);
					break;
				case Axis.Vertical:
					content.sizeDelta += new Vector2(0, cellSize.y + padding.x);
					break;
			}
		}
	}
}
