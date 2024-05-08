using UnityEngine;

namespace UnityUtils.GameObjects.Transforms
{
	public static class RectTransformUtils
	{
		public static void SimulateWorldPosition(this RectTransform rect, Vector3 worldposition, Camera camera)
		{
			Vector3 screenPoint = camera.WorldToScreenPoint(worldposition, Camera.MonoOrStereoscopicEye.Mono);
			screenPoint.z = 0;
			rect.position = screenPoint;
		}
	}
}
