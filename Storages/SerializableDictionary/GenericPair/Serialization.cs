using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serialized
{
	public partial class Dictionary<TKey, TValue, TSerializedPair> : ISerializationCallbackReceiver
		where TSerializedPair : IKeyValuePair<TKey, TValue>, new()
	{
		[SerializeField]
		private TSerializedPair[] pairs;

		public void OnAfterDeserialize()
		{
			dict = new();

			pairs ??= Array.Empty<TSerializedPair>();

			for (int i = 0; i < pairs.Length; i++)
			{
				TSerializedPair p = pairs[i];

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
			pairs = dict.Select(kp => new TSerializedPair() { Key = kp.Key, Value = kp.Value }).ToArray();
		}
	}
}
