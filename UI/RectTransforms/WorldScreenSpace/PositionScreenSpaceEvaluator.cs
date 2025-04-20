using System;
using UnityEngine;
using UnityUtils.GameObjects.Transforms;
using Utilities.Numbers;

namespace UnityUtils.UI.WorldScreenSpace
{
	[Serializable]
	public class PositionScreenSpaceEvaluator : IScreenSpaceEvaluator
	{
		public Vector3 position;
		public float size;

		public bool Update(Canvas canvas, RectTransform transform)
		{
			Camera camera = canvas.worldCamera;
			Vector2 pos = camera.WorldToScreenPosition(position, out Vector2 _, out float distance);
			if (distance <= 0)
			{
				transform.localScale = Vector3.zero;
				return false;
			}
			
			transform.position = pos;
			transform.localScale = Vector3.one * (size / distance);
			return true;
		}
	}
}
