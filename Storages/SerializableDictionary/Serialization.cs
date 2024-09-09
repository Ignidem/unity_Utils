using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serialized
{
	public partial class Dictionary<TKey, TValue> : ISerializationCallbackReceiver
	{
		[Serializable]
		public struct PairKey : IDictionaryElement<TKey>
		{
			[field: SerializeField]
			public TKey Key { get; set; }

			public TValue Value;
		}

		[SerializeField]
		private PairKey[] pairs;

		public void OnAfterDeserialize()
		{
			dict = new();

			pairs ??= new PairKey[0];

			for (int i = 0; i < pairs.Length; i++)
			{
				PairKey p = pairs[i];

				if (p.Key is null || (p.Key is string s && string.IsNullOrEmpty(s)) || dict.ContainsKey(p.Key))
					continue;

				dict.Add(p.Key, p.Value);
			}

			if (!Application.isEditor)
				pairs = null;
		}

		public void OnBeforeSerialize()
		{
			if (Application.isEditor)
				return;
			
			ApplyChanges();
		}

		public void ApplyChanges()
		{
			pairs = dict.Select(kp => new PairKey() { Key = kp.Key, Value = kp.Value }).ToArray();
		}
	}
}
