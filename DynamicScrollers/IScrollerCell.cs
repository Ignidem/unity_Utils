using System;
using UnityEngine;
using Axis = UnityEngine.RectTransform.Axis;

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
		RectTransform Transform { get; }

		Vector2 GetSize(Rect container, Axis axis);

		bool SetData(IScrollerCellData data);
		void Refresh();
		void Clear();
	}
}