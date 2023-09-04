namespace Serialized
{
	public partial class Dictionary<TKey, TValue>
	{
		public Dictionary(System.Collections.Generic.IDictionary<TKey, TValue> dictionary)
		{
			dict = new System.Collections.Generic.Dictionary<TKey, TValue>(dictionary);
		}

		public Dictionary(
			System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, TValue>> collection)
		{
			dict = new System.Collections.Generic.Dictionary<TKey, TValue>(collection);
		}

		public Dictionary(System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			dict = new System.Collections.Generic.Dictionary<TKey, TValue>(comparer);
		}

		public Dictionary(int capacity)
		{
			dict = new System.Collections.Generic.Dictionary<TKey, TValue>(capacity);
		}

		public Dictionary(System.Collections.Generic.IDictionary<TKey, TValue> dictionary, 
			System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			dict = new System.Collections.Generic.Dictionary<TKey, TValue>(dictionary, comparer);
		}

		public Dictionary(
			System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, TValue>> collection,
			System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			dict = new System.Collections.Generic.Dictionary<TKey, TValue>(collection, comparer);
		}

		public Dictionary(int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer)
		{
			dict = new System.Collections.Generic.Dictionary<TKey, TValue>(capacity, comparer);
		}
	}
}