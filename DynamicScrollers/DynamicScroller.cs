using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.DynamicScrollers
{
	[ExecuteInEditMode]
	public partial class DynamicScroller : ScrollRect
	{
		public static readonly string[] serializedFields =
		{
			nameof(cells),
		};

		[SerializeField] private Cells cells;

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
			}

			for (; dataIndex < _data.Length; dataIndex++)
			{
				IScrollerCellData data = _data[dataIndex];
				IScrollerCell currentCell = cells[cellIndex];
				if (currentCell == null)
				{
					if (cells.TryRecycleOrCreate(data, transform, out IScrollerCell cell))
						SetCell(cell);
				}
				else if (currentCell.CellType != data.CellType)
				{
					cells.RecycleAt(dataIndex);
					if (cells.TryRecycleOrCreate(data, transform, out IScrollerCell cell))
						SetCell(cell);
				}
				else
				{
					currentCell.Clear();
					currentCell.SetData(data);
					SetCell(currentCell);
				}
			}
		}
	}
}
