using UnityEngine;

namespace UnityUtils.Cameras
{
	public static class CameraUtils
	{
		public static Vector2 SizeAtDistance(this Camera cam, float distance)
		{
			Transform transform = cam.transform;
			Vector3 forward = transform.forward;

			Ray max = cam.ViewportPointToRay(new Vector2(1, 1), Camera.MonoOrStereoscopicEye.Mono);
			Ray min = cam.ViewportPointToRay(new Vector2(0, 0), Camera.MonoOrStereoscopicEye.Mono);

			float angle = Vector3.Angle(min.direction, forward);
			float d = (distance - cam.nearClipPlane) / Mathf.Cos(angle * Mathf.Deg2Rad);

			Vector3 maxPoint = max.GetPoint(d);
			Vector3 minPoint = min.GetPoint(d);

			Vector3 vect = Vector3.ProjectOnPlane(maxPoint - minPoint, forward);
			return new Vector2(vect.x, vect.z);
		}
	}
}
