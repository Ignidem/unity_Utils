using UnityUtils.Common.Layout;

namespace UnityUtils.Layouts.LayoutElementBehaviour
{
	public interface ILayoutElementBehaviour
	{
		void UpdateElement(IWorldGridLayoutElement element, LayoutElementInfo info);
	}
}
