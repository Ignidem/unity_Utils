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
		
		public static void SetLeft(this RectTransform rt, float left)
		{
			rt.offsetMin = new Vector2(left, rt.offsetMin.y);
		}

		public static void SetRight(this RectTransform rt, float right)
		{
			rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
		}

		public static void SetTop(this RectTransform rt, float top)
		{
			rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
		}

		public static void SetBottom(this RectTransform rt, float bottom)
		{
			rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
		}
	}
}
