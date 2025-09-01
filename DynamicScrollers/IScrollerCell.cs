using System;
using System.Threading.Tasks;
using UnityEngine;
using Axis = UnityEngine.RectTransform.Axis;

namespace UnityUtils.DynamicScrollers
{
	public interface IScrollerCellData
	{
		Type CellType { get; }
		bool IsSelected { get; set; }
		[Obsolete("Unused")]
		void Clear() { }
	}

	public interface IScrollerCell
	{
		Type CellType { get; }
		int CellIndex { get; set; }
		int DataIndex { get; set; }
		RectTransform Transform { get; }

		Vector2 GetSize(Rect container, Axis axis);

		Task<bool> SetData(IScrollerCellData data);
		void Refresh();
		void Clear();
	}

	public interface IScrollerCell<TData> : IScrollerCell
		where TData : IScrollerCellData
	{
		Task<bool> IScrollerCell.SetData(IScrollerCellData data)
		{
			return data is TData _data ? SetData(_data) : Task.FromResult(false);
		}

		Task<bool> SetData(TData data);
	}
}