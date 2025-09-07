using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtils.DynamicScrollers
{
	public class DataAlignmentSorter : IScrollerSorter
	{
		public enum DataOrder
		{
			Ascending,
			Descending
		}

		[SerializeField] private DataOrder order;

		public IEnumerable<int> Sort(IReadOnlyList<IScrollerCellData> data)
		{
			if (data == null) return Enumerable.Empty<int>();
			int count = data.Count;
			return order switch
			{
				DataOrder.Descending => Descending(count),
				_ => Ascending(count),
			};
		}
		private IEnumerable<int> Ascending(int count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return i;
			}
		}
		private IEnumerable<int> Descending(int count)
		{
			for (int i = count - 1; i >= 0; i--)
			{
				yield return i;
			}
		}
	}
}
