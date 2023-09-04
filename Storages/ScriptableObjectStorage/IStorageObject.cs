using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityUtils.Storages
{
	public interface IStorageObject
	{
		Type TableType { get; }
	}

	public class StorageObject<TKey, TValue> : ScriptableObject, IStorageObject
	{
		private class ValueDictionary : SerializedDictionary<TKey, TValue> { }

		public Type TableType => typeof(TValue);

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

		[SerializeReference]
		private ValueDictionary values;

		public void Delete(TKey key) => values.Remove(key);
	}
}
