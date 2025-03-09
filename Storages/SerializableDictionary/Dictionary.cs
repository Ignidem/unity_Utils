using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Serialized
{
	public class Dictionary<TKey, TValue> : Dictionary<TKey, TValue, Dictionary<TKey, TValue>.PairKey>
	{
		[Serializable]
		public struct PairKey : IKeyValuePair<TKey, TValue>
		{
			[field: SerializeField]
			public TKey Key { get; set; }

			[SerializeField]
			public TValue Value;

			TValue IKeyValuePair<TKey, TValue>.Value
			{
				get => Value;
				set => Value = value;
			}
		}
		
	}
}