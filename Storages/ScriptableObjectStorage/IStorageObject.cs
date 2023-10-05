using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityUtils.Storages
{
	public abstract class StorageObject : ScriptableObject
	{
		public abstract Type TableType { get; }

		public abstract T[] GetAllAs<T>();
	}

	public interface IStorageObject<TKey, TValue>
	{
		TValue this[TKey key] { get; set; }
		void Delete(TKey key);
	}

	public class StorageObject<TKey, TValue> : StorageObject, IStorageObject<TKey, TValue>
	{
		public override Type TableType => typeof(TValue);

		public TValue this[TKey key] 
		{ 
			get
			{
				return values.TryGetValue(key, out TValue value) ? value : default;
			}
			set
			{
				if (value == null)
				{
					Delete(key);
				}
				else
				{
					values[key] = value;
				}
			}
		}

		[SerializeField]
		private Serialized.Dictionary<TKey, TValue> values;

		public void Delete(TKey key) => values.Remove(key);

		public override T[] GetAllAs<T>()
		{
			return values.Values.Where(v => v is T).Cast<T>().ToArray();
		}
	}

	public class StorageObject<TValue> : StorageObject, IStorageObject<int, TValue>
	{
		public override Type TableType => typeof(TValue);

		public TValue this[int key]
		{
			get
			{
				if (key < 0 || key >= values.Count) return default;
				return values[key];
			}
			set
			{

				if (value == null)
				{
					Delete(key);
					return;
				}
				
				if (key < 0 || key >= values.Count) return;
				values[key] = value;
			}
		}

		[SerializeField]
		private List<TValue> values;

		public void Delete(int key)
		{
			if (key < 0 || key >= values.Count) return;
			values.RemoveAt(key);
		}

		public override T[] GetAllAs<T>()
		{
			return values.Where(v => v is T).Cast<T>().ToArray();
		}
	}
}
