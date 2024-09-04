using System;
using UnityEngine;

namespace UnityUtils.Serialization
{
	public class ScriptableType<T>
		where T : class
	{
		[Serializable]
		public class Reference : ScriptableObject
		{
			[SerializeField]
			public T Instance { get; private set; }
		}
	}
}
