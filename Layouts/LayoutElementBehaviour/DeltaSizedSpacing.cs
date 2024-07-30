using UnityEngine;
using UnityUtils.Common.Layout;

namespace UnityUtils.Layouts.LayoutElementBehaviour
{
	public struct DeltaSizedSpacing : ILayoutElementBehaviour
	{
		public readonly void UpdateElement(IWorldGridLayoutElement element, LayoutElementInfo info)
		{
			RectTransform transform = element.Transform as RectTransform;
			Vector2 overflowPercent = new Vector2(
				(float)info.actualGridSize.x / info.maxGridSize.x,
				(float)info.actualGridSize.y / info.maxGridSize.y);
			transform.sizeDelta = info.spacing * overflowPercent;
		}
	}
}
