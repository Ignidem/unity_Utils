using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.PropertyAttributes;
using UnityUtils.RectUtils;
using Axis = UnityEngine.RectTransform.Axis;

namespace UnityUtils.DynamicScrollers
{
	public partial class DynamicScroller : ScrollRect
	{
		private static IScrollerSorter DefaultSorter = new DataAlignmentSorter();
		public static readonly string[] serializedFields =
		{
			nameof(cells),
			nameof(contentComponents),
			nameof(filter),
			nameof(sorter),
		};

		[SerializeField] private Cells cells;
		
		[SerializeField] private ContentComponents contentComponents;

		private IReadOnlyList<IScrollerCellData> _data;
		public IReadOnlyList<IScrollerCellData> Data
		{
			get => _data;
			set
			{
				if (ReferenceEquals(_data, value)) return;

				_data = value;
				ReloadCells();
			}
		}

		public Axis ScrollAxis => vertical ? Axis.Vertical : Axis.Horizontal;

		[SerializeReference, Polymorphic(true)]
		private IScrollerFilter filter;

		[SerializeReference, Polymorphic(true)]
		private IScrollerSorter sorter;

		private bool pendingReload;

		protected override void OnEnable()
		{
			base.OnEnable();
			ReloadCells();
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();

			if (pendingReload)
			{
				pendingReload = false;
				ReloadCells();
			}
		}

		public void SetFilter(IScrollerFilter filter)
		{
			this.filter = filter;
		}
		public bool FilterData(int dataIndex, IScrollerCellData data)
		{
			return filter == null || filter.Include(dataIndex, data);
		}

		public void RequestReload()
		{
			pendingReload = true;
		}
		public void ReloadCells()
		{
			if (!Application.isPlaying) return;
			
			ResetContentSize();

			int cellIndex = 0;
			int count = Math.Max(_data?.Count ?? 0, cells.MinimumCount);
			if (count > 0)
			{
				foreach (int dataIndex in (sorter ?? DefaultSorter).Sort(_data))
				{
					if (ReloadAt(cellIndex, dataIndex))
						cellIndex++;
				}
			}

			//Padding Cells
			for (; cellIndex < count; cellIndex++)
			{
				if (!ReloadAt(cellIndex, -1))
					throw new Exception("Failed to reload empty cell");
			}
			
			for (int i = cells.Count - 1; i >= cellIndex; i--)
			{
				if (!cells.CacheCellAt(i, out IScrollerCell cell)) 
					continue;

				ClearCell(cell);
			}

			if (count > 0 && contentComponents.Sizing == ContentComponents.SizingType.OnReload && contentComponents.Layout != null)
			{
				Vector2 size = contentComponents.Layout.GetContentSize(ScrollAxis, viewport);
				SetContentSize(size);
			}
		}

		/*
		public bool ReloadDataAt(int dataIndex, IScrollerCellData data)
		{
			IScrollerCell cell = FindCellForDataAt(dataIndex);
			if (cell == null)
				return false;

			Data[dataIndex] = data;
			cell.SetData(data);
			return true;
		}//*/
		private bool ReloadAt(int cellIndex, int dataIndex)
		{
			IScrollerCellData data = DataAtOrDefault(dataIndex);

			if (!FilterData(dataIndex, data))
				return false;

			IScrollerCell cell = cells[cellIndex];

			Type cellType = data?.CellType ?? cells.GetDefaultCellType();
			
			if (cell != null && cell?.CellType == cellType)
			{
				ClearCell(cell);
				cell.SetData(data);
				InitializeCell(cell, cellIndex, dataIndex);
				return true;
			}

			if (cells.TryRecycleOrCreate(data, out cell))
			{
				InitializeCell(cell, cellIndex, dataIndex);
				return true;
			}

			return false;
		}

		private void ResetContentSize()
		{
			Axis scrollAxis = ScrollAxis;
			SetContentSize(contentComponents.StartPadding(scrollAxis) + contentComponents.EndPadding(scrollAxis));
		}

		private void SetContentSize(Vector2 size)
		{
			content.SetSize(size);
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

		public IScrollerCellData DataAtOrDefault(int dataIndex)
		{
			if (dataIndex < 0 || dataIndex >= _data.Count)
				return null;
			
			return _data[dataIndex];
		}
	}
}
