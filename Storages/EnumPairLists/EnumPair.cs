using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.Storages.EnumPairLists
{
	[Serializable]
	public class EnumPair<TEnum, TValue> : IEnumPairField,
		IEnumerable<KeyValuePair<TEnum, TValue>>, IEnumerable
		where TEnum : Enum
	{
		private static readonly TEnum[] enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
		private static readonly Dictionary<TEnum, int> indexer = CreateIndexer();
		private static Dictionary<TEnum, int> CreateIndexer()
		{
			TEnum[] values = enumValues;
			Dictionary<TEnum, int> indexes = new();
			for (int i = 0; i < values.Length; i++)
			{
				indexes.Add(values[i], i);
			}

			return indexes;
		}

		public Type EnumType { get; } = typeof(TEnum);
		public Type ValueType { get; } = typeof(TValue);
		public int Count => enumValues.Length;
		public IReadOnlyCollection<TEnum> EnumValues => enumValues;
		public IReadOnlyCollection<TValue> Values => values;

		public TValue this[TEnum key]
		{
			get => values[indexer[key]];
			set => values[indexer[key]] = value;
		}
		public (TEnum, TValue) this[int index] => (enumValues[index], values[index]);

		[SerializeField]
		private TValue[] values;

		public string GetNameAt(int index) => enumValues[index].ToString();

		private IEnumerable<KeyValuePair<TEnum, TValue>> Enumerate()
		{
			for (int i = 0; i < values.Length; i++)
				yield return new KeyValuePair<TEnum, TValue>(enumValues[i], values[i]);
		}
		public IEnumerator<KeyValuePair<TEnum, TValue>> GetEnumerator() => Enumerate().GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
