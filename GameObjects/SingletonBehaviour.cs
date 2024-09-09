using UnityEngine;

namespace UnityUtils.GameObjects
{
	public abstract class SingletonBehaviour<T> : MonoBehaviour
		where T : SingletonBehaviour<T>
	{
		public static T Singleton { get; private set; }

		[SerializeField] private bool destroyDuplicate;

		protected virtual void Awake()
		{
			if (Singleton && Singleton != this && destroyDuplicate)
			{
				Destroy(Singleton);
			}

			Singleton = (T)this;
		}

		protected virtual void OnDestroy()
		{
			if (this == Singleton)
				Singleton = default;
		}
	}
}
