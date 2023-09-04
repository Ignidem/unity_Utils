namespace Serialized
{
	public partial class Dictionary<TKey, TValue> : System.Collections.Generic.IDictionary<TKey, TValue>
	{
		public System.Collections.Generic.ICollection<TKey> Keys => dict.Keys;

		public System.Collections.Generic.ICollection<TValue> Values => dict.Values;

		public int Count => dict.Count;

		public bool IsReadOnly => false;

		public void Add(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
			=> dict.Add(item.Key, item.Value);

		public bool Contains(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
			=> (dict as System.Collections.Generic.IDictionary<TKey, TValue>).Contains(item);

		public void CopyTo(System.Collections.Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex)
			=> (dict as System.Collections.Generic.IDictionary<TKey, TValue>).CopyTo(array, arrayIndex);

		public bool Remove(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
			=> (dict as System.Collections.Generic.IDictionary<TKey, TValue>).Remove(item);
	}
}
