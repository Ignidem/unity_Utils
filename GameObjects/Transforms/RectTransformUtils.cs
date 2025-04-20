using UnityEngine;

namespace UnityUtils.GameObjects.Transforms
{
	public static class RectTransformUtils
	{
		public static Vector2 WorldToScreenPosition(this Camera camera, Vector3 worldposition, 
			out Vector2 normalizedPoint, out float distance)
		{
			Vector2 size = camera.ViewportToScreenPoint(Vector2.one);
			Vector3 screen = camera.WorldToScreenPoint(worldposition, Camera.MonoOrStereoscopicEye.Mono);
			distance = screen.z;
			normalizedPoint = new Vector2(screen.x / size.x, screen.y / size.y);
			return new Vector2(screen.x, screen.y);
		}

		public static Rect GetPivotOffset(this RectTransform transform) => transform.GetPivotOffset(transform.position);
		public static Rect GetPivotOffset(this RectTransform transform, Vector2 position)
		{
			Vector2 elementSize = transform.rect.size;
			Vector2 pivotOffset = elementSize * transform.pivot;
			Vector2 bottomLeft = position - pivotOffset;
			return new Rect(bottomLeft, elementSize);
		}
	}
}
