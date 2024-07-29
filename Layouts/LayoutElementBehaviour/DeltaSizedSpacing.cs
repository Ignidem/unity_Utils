using UnityEngine;
using UnityUtils.Common.Layout;

namespace UnityUtils.Layouts.LayoutElementBehaviour
{
	public readonly struct DeltaSizedSpacing : ILayoutElementBehaviour
	{
		public readonly void UpdateElement(IWorldGridLayoutElement element, LayoutElementInfo info)
		{
			RectTransform transform = element.Transform as RectTransform;
			transform.sizeDelta = info.spacing;
		}
	}
}
