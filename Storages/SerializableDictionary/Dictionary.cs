using System;
using System.Runtime.Serialization;

namespace Serialized
{
	[Serializable]
	public partial class Dictionary<TKey, TValue>
	{
		public static explicit operator System.Collections.Generic.Dictionary<TKey, TValue>
			(Dictionary<TKey, TValue> sdict) => sdict.dict;

		public static explicit operator Dictionary<TKey, TValue>
			(System.Collections.Generic.Dictionary<TKey, TValue> dict) => new(dict);

		private System.Collections.Generic.Dictionary<TKey, TValue> dict;

		public TValue this[TKey key]
		{
			get => dict.TryGetValue(key, out TValue value) ? value : default;
			set
			{
				if (value == null)
				{
					dict.Remove(key);
				}
				else
				{
					dict[key] = value;
				}
			}
		}

		public Dictionary()
		{
			dict = new();
		}

		public void Add(TKey key, TValue value) => dict.Add(key, value);
		public void Clear() => dict.Clear();
		public bool ContainsKey(TKey key) => dict.ContainsKey(key);
		public bool ContainsValue(TValue value) => dict.ContainsValue(value);
		public int EnsureCapacity(int capacity) => dict.EnsureCapacity(capacity);
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
			=> dict.GetObjectData(info, context);
		public virtual void OnDeserialization(object sender) => dict.OnDeserialization(sender);
		public bool Remove(TKey key, out TValue value) => dict.Remove(key, out value);
		public bool Remove(TKey key) => dict.Remove(key);
		public void TrimExcess() => dict.TrimExcess();
		public void TrimExcess(int capacity) => dict.TrimExcess(capacity);
		public bool TryAdd(TKey key, TValue value) => dict.TryAdd(key, value);
		public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);
	}
}
