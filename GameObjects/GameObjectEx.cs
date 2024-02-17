using UnityEngine;

namespace UnityUtils.GameObjects
{
	public static class GameObjectEx
	{
		public static void DestroySelf(this Object obj, float delay = 0)
		{
			if (!obj) return;

			if (Application.isPlaying)
				Object.Destroy(obj, delay);
			else
				Object.DestroyImmediate(obj);
		}

		public static void DestroySelfObject(this Object obj, float delay = 0)
		{
			if (obj is Component comp && comp)
			{
				comp.gameObject.DestroySelf(delay);
			}
			else
			{
				obj.DestroySelf(delay);
			}
		}

		public static bool TryGetInSelfOrChildren<T>(this GameObject obj, out T comp)
		{
			if (!obj.TryGetComponent(out comp))
				comp = obj.GetComponentInChildren<T>();

			return comp != null;
		}
	}
}
