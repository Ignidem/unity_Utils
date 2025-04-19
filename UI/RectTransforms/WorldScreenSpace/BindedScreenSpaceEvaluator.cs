using System;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils.GameObjects.Transforms;

namespace UnityUtils.UI.WorldScreenSpace
{
	[Serializable]
	public class BindedScreenSpaceEvaluator : IScreenSpaceEvaluator
	{
		public bool IsTargetValid => target != null && target && target.gameObject.activeSelf;
		public Transform target;
		public Vector3 offset;
		public Bounds normalizedBounds = new Bounds(Vector3.zero, Vector3.one);

		public bool Update(Camera camera, RectTransform transform)
		{
			Vector3 worldPos = target.transform.position + offset;
			Vector2 pos = camera.WorldToScreenPosition(worldPos, out Vector2 norm, out float distance);
			if (distance <= 0 || !normalizedBounds.Contains(norm)) return false;
			
			transform.position = pos;
			return true;
		}
	}
}
