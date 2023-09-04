using System.Collections;

namespace Serialized
{
	public partial class Dictionary<TKey, TValue> : 
		System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, TValue>>, IEnumerable
	{
		public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<TKey, TValue>> GetEnumerator()
			=> dict.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
			=> dict.GetEnumerator();
	}
}
