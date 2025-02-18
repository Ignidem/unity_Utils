using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.Utils.Versioning
{
	public class CompileDefineCollection : ScriptableObject, IEnumerable<string>
	{
		[SerializeField] private string[] defines;

		public IEnumerator<string> GetEnumerator()
		{
			IEnumerable<string> values = defines;
			return values.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
