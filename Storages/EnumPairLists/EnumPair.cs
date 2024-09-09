using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.Storages.EnumPairLists
{
	[Serializable]
	public class EnumPair<TEnum, TValue> : EnumCollection<TEnum>, IEnumPairField,
		IEnumerable<KeyValuePair<TEnum, TValue>>, IEnumerable
		where TEnum : Enum
	{
		public Type EnumType { get; } = typeof(TEnum);
		public Type ValueType { get; } = typeof(TValue);
		public int Count => EnumValues.Count;
		public IReadOnlyList<TValue> Values => values;

		public TValue this[TEnum key]
		{
			get => values[Indexer[key]];
			set => values[Indexer[key]] = value;
		}
		public (TEnum key, TValue value) this[int index] => (EnumValues[index], values[index]);

		[SerializeField]
		private TValue[] values;

		public string GetNameAt(int index) => EnumValues[index].ToString();

		private IEnumerable<KeyValuePair<TEnum, TValue>> Enumerate()
		{
			int vCount = values.Length;
			int eCount = Count;
			for (int i = 0; i < eCount; i++)
			{
				TEnum key = EnumValues[i];
				TValue value = i >= vCount ? default : values[i];
				yield return new KeyValuePair<TEnum, TValue>(key, value);
			}
		}
		public IEnumerator<KeyValuePair<TEnum, TValue>> GetEnumerator() => Enumerate().GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
