using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils.Layouts.SpacingEvaluators;
using UnityUtils.PropertyAttributes;
using Maths = UnityEngine.Mathf;
using Utils.Asyncronous;

namespace UnityUtils.Common.Layout
{
	[ExecuteInEditMode]
	public class WorldGridLayout : MonoBehaviour
	{
		public bool AutoReload;

		public Vector3Int Directions;

		public Vector3 Offset;

		[SerializeField, HideInInspector] private Vector3 MinSpacing;
		[SerializeField, HideInInspector] private Vector3 MaxSpacing;

		[SerializeReference, Polymorphic]
		public ISpacingEvaluator spacing;

		public Vector3Int GridSize;

		[Header("Overflow Axis")]
		[InspectorName("X")]
		public bool overflowX;
		[InspectorName("Y")]
		public bool overflowY;
		[InspectorName("Z")]
		public bool overflowZ;

		[Header("Events")]
		public UnityEvent<Transform, Vector3, int, int> OnChildReloaded;

		public int MaxElements => Math.Max(1, GridSize.x) * Math.Max(1, GridSize.y) * Math.Max(1, GridSize.z);
		public bool HasOverflow => overflowX || overflowY || overflowZ;

		private TaskDelayer reloadDelayer;

		private void OnValidate()
		{
			int overflowCount = (overflowX ? 1 : 0) + (overflowY ? 1 : 0) + (overflowZ ? 1 : 0);
			if (overflowCount > 1)
			{
				Debug.LogWarning("Only one overflow axis is supported at the moment. " +
					"Only the first active overflow axis will be applied.");
			}

			GridSize = new Vector3Int(
				Math.Max(1, GridSize.x),
				Math.Max(1, GridSize.y),
				Math.Max(1, GridSize.z)
				);

			spacing ??= new DefinedSpacing()
			{
				MinSpacing = this.MinSpacing,
				MaxSpacing = this.MaxSpacing,
			};

			OnAutoReload();
		}

		private void Awake()
		{
			reloadDelayer = new TaskDelayer(50, ReloadNext);
		}
		private void OnEnable()
		{
			OnAutoReload();
		}

		private void OnRectTransformDimensionsChange()
		{
			OnAutoReload();
		}

		private void OnTransformChildrenChanged()
		{
			OnAutoReload();
		}

		private void OnAutoReload()
		{
			if (!AutoReload) return;

			if (reloadDelayer != null) 
			{
				reloadDelayer.TryRun();
			}
#if UNITY_EDITOR
			else
			{
				_ = ReloadNext();
			}
#endif
		}
		public async Task ReloadNext()
		{
			await Task.Yield();
			ReloadLayout();
		}

		public void ReloadLayout()
		{
			if (!this || !enabled) return;

			int count = transform.childCount;
			Vector3 spacing = GetSpacing();
			Vector3Int grid = GetFittingGridSize(count);

			for (int i = 0; i < count; i++)
			{
				Transform child = transform.GetChild(i);

				Vector3Int gridPos = GetChildGridPosition(i, grid);
				Vector3 pos = GetLocalPositionOfChild(gridPos, spacing, grid);
				child.localPosition = pos;

				OnChildReloaded?.Invoke(child, pos, i, count);
				if (child.TryGetComponent(out IWorldGridLayoutElement element))
				{
					LayoutElementInfo info = new LayoutElementInfo(i, gridPos, grid, GridSize, pos, spacing);
					element.SetLayoutPosition(info);
				}
			}
		}

		public Vector3 GetSpacing()
		{
			return spacing.GetSpacing(transform.childCount, this);
		}

		public void InsertChildren(Transform child, int index)
		{
			index = Math.Clamp(index, 0, transform.childCount);
			child.SetParent(transform, true);
			child.SetSiblingIndex(index);
			OnAutoReload();
		}

		public Vector3 AddChildren(Transform child)
		{
			child.SetParent(transform, true);
			int count = transform.childCount;
			Vector3 spacing = this.spacing.GetSpacing(count, this);
			Vector3Int grid = GetFittingGridSize(count);
			Vector3Int position = GetChildGridPosition(count - 1, grid);
			return GetLocalPositionOfChild(position, spacing, grid);
		}

		public Vector3Int GetFittingGridSize(int count)
		{
			if (count < MaxElements)
			{
				if (count <= GridSize.x)
					return new Vector3Int(count, 1, 1);

				int xy = GridSize.x * GridSize.y;
				if (count <= xy)
				{
					int columns = Maths.CeilToInt((float)count / GridSize.x);
					return new Vector3Int(GridSize.x, columns, 1);
				}

				int depth = Maths.CeilToInt((float)count / xy);
				return new Vector3Int(GridSize.x, GridSize.y, depth);
			}

			if (!HasOverflow)
				return GridSize;

			int x = GridSize.x, y = GridSize.y, z = GridSize.z;
			int extra = count - MaxElements;

			int SingleOverflow(int a, int b)
			{
				float ab = a * b;
				return Maths.CeilToInt(extra / ab);
			}

			//TODO! Support multiple axis overflow.

			/*
			bool FitsOneZStep()
			{
				if (extra == x * z)
				{
					x += 1;
					return true;
				}

				int one = (x + y + 1) * z;
				if (extra <= one)
				{
					x += 1;
					y += 1;
					return true;
				}

				return false;
			}

			void FullOverflow()
			{
				if (FitsOneZStep())
					return;

				int oneZ = ((x + y + 1) * z * 2) + (x * y);

				if (extra <= oneZ)
				{
					x += 1;
					y += 1;
					z += 1;
					return;
				}

				float u = extra - ((x + y) * z);
				float d = (2 * z) + (x * y);

				int steps = Maths.CeilToInt(u / d);

				x += steps;
				y += steps;
				z += steps;
			}

			if (overflowX && overflowY && overflowZ)
			{
				FullOverflow();
			}
			else if (overflowX && overflowY)
			{
				if (!FitsOneZStep())
				{
					int stepCount = Maths.CeilToInt(((extra/z) - x - y) / 2);
					x += stepCount;
					y += stepCount;
				}
			}
			else if (overflowX && overflowZ)
			{
				int steps = Maths.CeilToInt(extra / (z * y));
			}
			else if (overflowY && overflowZ)
			{
				
			}
			else*/ if (overflowX)
			{
				x += SingleOverflow(y, z);
			}
			else if (overflowY)
			{
				y += SingleOverflow(x, z);
			}
			else if (overflowZ)
			{
				z += SingleOverflow(x, y);
			}

			return new Vector3Int(x, y, z);
		}

		public Vector3Int GetChildGridPosition(int i, Vector3Int grid)
		{
			if (i < grid.x)
				return new Vector3Int(i, 0, 0);

			int x, y;
			int zSize = grid.x * grid.y;
			if (i < zSize)
			{
				y = i / grid.x;
				x = i - (y * grid.x);
				return new Vector3Int(x, y, 0); 
			}

			if (i < zSize * grid.z)
			{
				int z = i / zSize;
				int zw = z * zSize;
				y = (i - zw) / grid.x;
				int yw = y * grid.x;
				x = i - (yw + zw);

				return new Vector3Int(x, y, z);
			}

			return grid - Vector3Int.one;
		}

		public Vector3 GetLocalPositionOfChild(int i, int count)
		{
			Vector3 spacing = this.spacing.GetSpacing(count, this);
			Vector3Int grid = GetFittingGridSize(count);
			Vector3Int position = GetChildGridPosition(i, grid);
			return GetLocalPositionOfChild(position, spacing, grid);
		}

		public Vector3 GetLocalPositionOfChild(Vector3Int position, Vector3 spacing, Vector3Int grid)
		{
			float AxisPosition(int axis)
			{
				float dir = Directions[axis];
				int pos = position[axis];
				float space = spacing[axis];

				if (dir != 0)
					return pos * dir * space;

				int length = grid[axis] - 1;
				float centerOffset = length switch
				{
					0 or 1 => 0,
					2 => 0.5f,
					_ => length * 0.5f
				};
				return (pos * space) - (space * centerOffset);
			}

			Vector3 localPosition = new Vector3(AxisPosition(0), AxisPosition(1), AxisPosition(2));
			return localPosition + Offset;
		}
	}
}
