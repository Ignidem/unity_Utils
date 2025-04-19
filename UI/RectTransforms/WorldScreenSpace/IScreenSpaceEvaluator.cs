using UnityEngine;

namespace UnityUtils.UI.WorldScreenSpace
{
	public interface IScreenSpaceEvaluator
	{
		/// <summary>
		/// Updates the 2D UI element's position to match the projected world space onto a camera. 
		/// </summary>
		/// <param name="camera">The camera with which to project the target's world space.</param>
		/// <param name="transform">The rect transform of the UI element.</param>
		/// <returns>Whether the projected position is visible to the camera.</returns>
		bool Update(Camera camera, RectTransform transform);
	}
}
