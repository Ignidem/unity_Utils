using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityUtils.Storages.EnumPairLists
{
	public class EnumPairObject<TEnum, TValue> : ScriptableObject, IEnumerable<KeyValuePair<TEnum, TValue>>
		where TEnum : Enum
	{
		public TValue this[TEnum key]
		{
			get => pairs[key];
			set => pairs[key] = value;
		}
		public (TEnum key, TValue value) this[int index] => pairs[index];

		[SerializeField]
		private EnumPair<TEnum, TValue> pairs;

		public IEnumerator<KeyValuePair<TEnum, TValue>> GetEnumerator()
		{
			return pairs.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
