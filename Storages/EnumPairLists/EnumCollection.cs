using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityUtils.Storages.EnumPairLists
{
	public class EnumCollection<TEnum>
	{
		public static IReadOnlyList<TEnum> EnumValues => enumValues;
		private static readonly TEnum[] enumValues = ((TEnum[])Enum.GetValues(typeof(TEnum))).Distinct().ToArray();
		public static IReadOnlyDictionary<TEnum, int> Indexer => indexer;
		private static readonly Dictionary<TEnum, int> indexer = CreateIndexer();
		private static Dictionary<TEnum, int> CreateIndexer()
		{
			TEnum[] values = enumValues;
			Dictionary<TEnum, int> indexes = new();
			for (int i = 0; i < values.Length; i++)
			{
				if (indexes.ContainsKey(values[i]))
					continue;

				indexes[values[i]] = i;
			}

			return indexes;
		}
	}
}
