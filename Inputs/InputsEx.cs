using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.Inputs
{
	public static class InputsEx
	{
		public static bool TryRaycast(this Camera camera, Vector2 screenPosition, out RaycastHit hit)
		{
			if (!camera)
			{
				if (!Camera.main)
				{
					hit = default;
					return false;
				}

				camera = Camera.main;
			}

			Ray ray = camera.ScreenPointToRay(screenPosition);

			return Physics.Raycast(ray, out hit);
		}

		public static bool TryRaycast<T>(this Camera camera, Vector2 screenPosition, out RaycastHit hit, out T target)
		{
			if (!camera.TryRaycast(screenPosition, out hit))
			{
				target = default;
				return false;
			}

			return hit.collider.gameObject.TryGetComponent(out target);
		}
	
		public static bool Drop<T>(this T item, Vector2 pointerScreenPosition, Camera camera = null)
		{
			if (!camera.TryRaycast(pointerScreenPosition, out _, out IDropHandler<T> handler))
				return false;

			return handler.OnDrop(item);
		}

		public static async Task<bool> DropAsync<T>(this T item, Vector2 pointerScreenPosition, Camera camera = null)
		{
			if (!camera.TryRaycast(pointerScreenPosition, out _, out IDropHandlerAsync<T> handler))
				return false;

			return await handler.OnDrop(item);
		}
	}
}
