using UnityEngine;

namespace UnityUtils.Inputs
{
	public struct RaycastOptions
	{
		public static implicit operator RaycastOptions(Vector2 pos) => new RaycastOptions(pos);

		public Vector3 screenPosition;
		public int maxDistance;
		public LayerMask layerMask;

		public RaycastOptions(Vector2 screenPosition, LayerMask? layerMask = null, int? maximumDistance = null)
		{
			this.screenPosition = screenPosition;
			maxDistance = maximumDistance ?? int.MaxValue;
			this.layerMask = layerMask ?? 0;
		}
	}
}
