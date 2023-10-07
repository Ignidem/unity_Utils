using UnityEngine;

namespace UnityUtils.GameObjects
{
	public static class GameObjectEx
	{
		public static void DestroySelf(this GameObject go, float delay = 0)
		{
			if (!go) return;

			if (Application.isPlaying)
				Object.Destroy(go, delay);
			else
				Object.DestroyImmediate(go);
		}

		public static void DestroyGameObject(this Component component, float delay = 0)
		{
			if (component == null || !component.gameObject) return;
			component.gameObject.DestroySelf(delay);
		}
	}
}
