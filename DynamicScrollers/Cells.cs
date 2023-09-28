using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtils.DynamicScrollers
{
	public partial class DynamicScroller
	{
		[Serializable]
		public class Cells
		{
			public IScrollerCell this[int i]
			{
				get => i < 0 || i >= activeCells.Count ? null : activeCells[i];
				set
				{
					if (i >= activeCells.Count)
					{
						activeCells.Add(value);
					}
					else
					{
						activeCells[i] = value;
					}
				}
			}

			[SerializeField]
			private GameObject[] cellPrefabs;
			internal int prefabCount => cellPrefabs.Length;
			private readonly Dictionary<Type, GameObject> mappedPrefabs = new();

			private readonly List<IScrollerCell> activeCells = new();
			private readonly Dictionary<Type, List<IScrollerCell>> cachedCells = new();

			public int Count => activeCells.Count;

			public bool TryGetPrefab(IScrollerCellData data, out GameObject prefab)
			{
				Type type = data.CellType;
				if (!mappedPrefabs.TryGetValue(type, out prefab))
				{
					prefab = cellPrefabs.FirstOrDefault(p => p.GetComponent(type));
					if (!prefab) return false;
					mappedPrefabs[type] = prefab;
				}

				return true;
			}

			public bool TryRecycleOrCreate(IScrollerCellData data, Transform parent, out IScrollerCell cell)
			{
				if (!TryRecycle(data, out cell) && !TryCreate(data, parent, out cell))
					return false;

				cell.SetData(data);
				return true;
			}

			public void RecycleAt(int index)
			{
				if (index < 0 || index >= activeCells.Count) return;

				IScrollerCell cell = activeCells[index];
				activeCells.RemoveAt(index);
				Type type = cell.CellType;
				if (!cachedCells.TryGetValue(type, out List<IScrollerCell> cells))
					cells = cachedCells[type] = new List<IScrollerCell>();

				cell.Clear();
				cell.Transform.gameObject.SetActive(false);
				cells.Add(cell);
			}

			private bool TryRecycle(IScrollerCellData data, out IScrollerCell cell)
			{
				if (!cachedCells.TryGetValue(data.CellType, out var cells) || cells.Count == 0)
				{
					cell = null;
					return false;
				}

				int index = cells.Count - 1;
				cell = cells[index];
				cells.RemoveAt(index);
				cell.Transform.gameObject.SetActive(true);
				return true;
			}

			private bool TryCreate(IScrollerCellData data, Transform parent, out IScrollerCell cell)
			{
				if (!TryGetPrefab(data, out GameObject prefab))
				{
					cell = null;
					return false;
				}

				GameObject inst = Instantiate(prefab, parent);
				if (!inst.TryGetComponent(out cell))
				{
					Destroy(inst);
					return false;
				}

				return true;
			}
		}
	}
}
