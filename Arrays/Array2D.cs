using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtils.Arrays
{
	public struct Array2DElement<T>
	{
		public int x;
		public int y;
		public T value;

		public Array2DElement(int x, int y, T value) : this()
		{
			this.x = x;
			this.y = y;
			this.value = value;
		}
	}

	public interface IArray2D : IEnumerable
	{
		object this[int x, int y] { get; set; }
		Vector2Int Size { get; set; }
	}

	[Serializable]
	public class Array2D<T> : IArray2D, IEnumerable<T>, IEnumerable<Array2DElement<T>>
	{
		public const string ElementsFieldName = nameof(elements);

		public abstract class EnumeratorBase<K> : IEnumerator
		{
			private readonly Vector2Int size;
			protected SubArray[] elements;
			protected int x = 0;
			protected int y = -1;

			public abstract K Current { get; }
			object IEnumerator.Current => Current;

			public EnumeratorBase(SubArray[] elements, Vector2Int size)
			{
				this.elements = elements;
				this.size = size - Vector2Int.one;
			}

			public void Dispose()
			{
				elements = null;
			}

			public bool MoveNext()
			{
				if (elements == null) return false;

				while (elements[x].IsEmpty)
				{
					x++;
					y = 0;
					if (x > size.x) return false;
				}

				if (y < size.y)
				{
					y++;
					return true;
				}

				x++;
				y = 0;
				return x <= size.x;
			}

			public void Reset()
			{
				x = 0;
				y = 0;
			}
		}

		public class GenericEnumerator : EnumeratorBase<T>, IEnumerator<T>, IEnumerator<Array2DElement<T>>
		{
			public override T Current => elements[x][y];
			Array2DElement<T> IEnumerator<Array2DElement<T>>.Current => new Array2DElement<T>(x, y, Current);
			public GenericEnumerator(SubArray[] elements, Vector2Int size) : base(elements, size)
			{
			}
		}

		[Serializable]
		public class SubArray
		{
			public static implicit operator bool(SubArray array) => !array.IsEmpty;
			public static implicit operator T[](SubArray array) => array?.elements;
			public static implicit operator SubArray(T[] array) => new SubArray() { elements = array };

			public T this[int y]
			{
				get => elements[y];
				set => elements[y] = value;
			}

			public int Length => elements?.Length ?? 0;
			public bool IsEmpty => elements == null || Length == 0;

			public T[] elements;
		}

		public static implicit operator Array2D<T>(T[,] array)
		{
			Vector2Int size = new Vector2Int(array.GetLength(0), array.GetLength(1));
			Array2D<T> array2D = new Array2D<T>
			{
				elements = new SubArray[size.x]
			};

			for (int x = 0; x < size.x; x++) 
			{
				array2D.elements[x] = new T[size.y];
				for (int y = 0; y < size.y; y++)
				{
					array2D[x, y] = array[x, y];
				} 
			}

			return array2D;
		}

		public static implicit operator T[,](Array2D<T> array) 
		{
			var size = array.Size;
			T[,] mArray = new T[size.x, size.y];
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					mArray[x, y] = array[x, y];
				}
			}

			return mArray;
		}

		object IArray2D.this[int x, int y]
		{
			get => this[x, y];
			set
			{
				if (value is not T t) return;
				this[x, y] = t;
			}
		}

		public T this[int x, int y]
		{
			get => elements[x][y];
			set => elements[x][y] = value;
		}

		public Vector2Int Size
		{
			get => elements == null ? Vector2Int.zero : new Vector2Int(elements.Length, elements[0].Length);
			set => Resize(value);
		}

		[SerializeField] private SubArray[] elements;

		public IEnumerator<T> GetEnumerator()
		{
			if (Size == Vector2Int.zero)
				return Enumerable.Empty<T>().GetEnumerator();

			return new GenericEnumerator(elements, Size);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IEnumerator<Array2DElement<T>> IEnumerable<Array2DElement<T>>.GetEnumerator()
		{
			if (Size == Vector2Int.zero)
				return Enumerable.Empty<Array2DElement<T>>().GetEnumerator();

			return new GenericEnumerator(elements, Size);
		}

		private void Resize(Vector2Int size)
		{
			if (size == Size) return;

			if (size == Vector2Int.zero)
			{
				elements = null;
				return;
			}

			if (elements == null)
			{
				elements = new SubArray[size.x];
			}
			else
			{
				Array.Resize(ref elements, size.x);
			}

			int y = size.y;

			for (int x = 0; x < size.x; x++)
			{
				T[] sub = elements[x];
				if (sub == null)
				{
					elements[x] = new T[y];
				}
				else
				{
					Array.Resize(ref sub, y);
					elements[x] = sub;
				}
			}
		}
	}
}
