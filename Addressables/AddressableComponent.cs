using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.AddressableUtils
{
	[System.Serializable]
	public class AddressableComponent<T> : AddressableReference<GameObject>
	{
		public Task<T> InstantiateAs(Transform parent = null)
		{
			return base.InstantiateAs<T>(parent);
		}
	}
}
