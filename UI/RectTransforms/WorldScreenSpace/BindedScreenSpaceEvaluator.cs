using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils.GameObjects.Transforms;

namespace UnityUtils.UI.WorldScreenSpace
{
	[Serializable]
	public class BindedScreenSpaceEvaluator : IScreenSpaceEvaluator
	{
		public bool IsTargetValid => target != null && target && target.gameObject.activeSelf;
		public Transform target;
		[FormerlySerializedAs("offset")] public Vector3 worldOffset;
		public Vector2 screenOffset;
		public Bounds normalizedBounds = new Bounds(Vector3.zero, Vector3.one);

		public bool Update(Canvas canvas, RectTransform transform)
		{
			Vector3 worldPos = target.transform.position + worldOffset;
			Vector3 screenPos = canvas.worldCamera.WorldToScreenPoint(worldPos, Camera.MonoOrStereoscopicEye.Mono);
			float distance = screenPos.z;
			
			if (distance <= 0) return false;
			
			Rect element = transform.GetPivotOffset((Vector2)screenPos + screenOffset);
			if (!canvas.pixelRect.Overlaps(element)) return false;
			
			transform.position = screenPos;
			return true;
		}
	}
}
