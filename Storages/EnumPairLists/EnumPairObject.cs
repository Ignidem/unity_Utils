using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityUtils.Storages.EnumPairLists
{
	public class EnumPairObject<TEnum, TValue> : ScriptableObject
		where TEnum : Enum
	{
		public TValue this[TEnum key]
		{
			get => pairs[key];
			set => pairs[key] = value;
		}
		public (TEnum, TValue) this[int index] => pairs[index];

		[SerializeField]
		private EnumPair<TEnum, TValue> pairs;
	}
}
