using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Collections;

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
					if (i >= Count)
					{
						activeCells.Add(value);
						return;
					}

					IScrollerCell oldCell = this[i];
					if (oldCell == value)
						return;

					activeCells[i] = value;
					CacheCell(oldCell);
				}
			}

			public event CellDelegate OnCellCreated;

			[SerializeField, Tooltip("The index of cell type to use for cells with null data (padding cells)")]
			private int defaultPrefabIndex;

			[SerializeField]
			private GameObject[] cellPrefabs;
			
			[SerializeField]
			private RectTransform cellParent;

			[SerializeField]
			private RectTransform cellCache;
			
			[field: SerializeField, Tooltip("The minimum amount of cell to have. Default cells will be added to fill up to that count.")] 
			public int MinimumCount { get; private set; }

			internal int PrefabCount => cellPrefabs.Length;
			private readonly Dictionary<Type, GameObject> mappedPrefabs = new();

			private readonly List<IScrollerCell> activeCells = new();
			private readonly Dictionary<Type, List<IScrollerCell>> cachedCells = new();

			public int Count => activeCells.Count;

			public Type GetDefaultCellType()
			{
				if (defaultPrefabIndex < 0 || defaultPrefabIndex >= PrefabCount)
					throw new IndexOutOfRangeException($"The cells default prefab index '{defaultPrefabIndex}' is out of range of prefab list.");
				
				return GetPrefabComponentOrThrow(cellPrefabs[defaultPrefabIndex]).CellType;
			}

			public bool TryGetPrefab(IScrollerCellData data, out GameObject prefab)
			{
				Type type = data?.CellType ?? GetDefaultCellType();
				if (!mappedPrefabs.TryGetValue(type, out prefab))
				{
					prefab = cellPrefabs.FirstOrDefault(p => GetPrefabComponentOrThrow(p).CellType == type);
					if (!prefab) return false;
					mappedPrefabs[type] = prefab;
				}

				return true;
			}

			public bool TryRecycleOrCreate(IScrollerCellData data, out IScrollerCell cell)
			{
				return TryRecycle(data, out cell) || TryCreate(data, out cell);
			}

			public bool CacheCellAt(int index, out IScrollerCell cell)
			{
				if (index < 0 || index >= activeCells.Count)
				{
					cell = null;
					return false;
				}

				cell = activeCells.PopAt(index);

				CacheCell(cell);
				return true;
			}
			private void CacheCell(IScrollerCell cell)
			{
				Type type = cell.CellType;
				if (!cachedCells.TryGetValue(type, out List<IScrollerCell> cache))
					cache = cachedCells[type] = new List<IScrollerCell>();

				cache.Add(cell);
				if (cellCache) cell.Transform.SetParent(cellCache);
				cell.Transform.gameObject.SetActive(false);
			}

			private IScrollerCell GetPrefabComponentOrThrow(GameObject prefab)
			{
				if (prefab.TryGetComponent(out IScrollerCell cell))
					return cell;
				
				throw new Exception($"Prefab GameObject {prefab.name} does not have a component implementing IScrollerCell");
			}

			private bool TryRecycle(IScrollerCellData data, out IScrollerCell cell)
			{
				Type cellType = data?.CellType ?? GetDefaultCellType();
				if (!cachedCells.TryGetValue(cellType, out List<IScrollerCell> cache) || cache.Count == 0)
				{
					cell = null;
					return false;
				}

				cell = cache.Pop();
				if (cellCache) cell.Transform.SetParent(cellParent);
				cell.Transform.gameObject.SetActive(true);
				return true;
			}

			private bool TryCreate(IScrollerCellData data, out IScrollerCell cell)
			{
				if (!TryGetPrefab(data, out GameObject prefab))
				{
					cell = null;
					return false;
				}

				GameObject inst = Instantiate(prefab, cellParent);
				if (!inst.TryGetComponent(out cell))
				{
					Destroy(inst);
					return false;
				}

				OnCellCreated?.Invoke(cell);
				return true;
			}
		}

		public IScrollerCell GetCellAt(int index) => cells[index];
		public IScrollerCell FindCellForDataAt(int index)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				IScrollerCell cell = cells[i];
				if (cell.DataIndex == index)
					return cell;
			}

			return null;
		}
	}
}
