using UnityEngine;

namespace UnityUtils.GameObjects
{
	[System.Serializable]
	public class Contained<T>
		where T : UnityEngine.Object
	{
		public static implicit operator Transform(Contained<T> cnt) => cnt.Container;
		public static implicit operator T(Contained<T> cnt) => cnt.Target;

		[field: SerializeField] public Transform Container { get; private set; }
		[field: SerializeField] public T Target { get; private set; }

		public bool ContainerActive
		{
			get => Container.gameObject.activeSelf;
			set => Container.gameObject.SetActive(value);
		}
	}
}
