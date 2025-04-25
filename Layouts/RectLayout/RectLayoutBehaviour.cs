using UnityEngine;
using UnityUtils.Common.Layout;

namespace UnityUtils.Layouts.RectLayout
{
	public class RectLayoutBehaviour : LayoutController, IRectLayoutElement
	{
		public bool IsEnabled => enabled;
		public RectTransform Transform => settings.Transform;
		public Vector2Int OffsetDirection => settings.OffsetDirection;

		[SerializeField] private Transform sizeSource;
		[SerializeField] private RectLayoutComponent settings;

		public override void ReloadLayout(bool animate)
		{
			base.ReloadLayout(animate);
			settings?.Reload(sizeSource ? sizeSource : transform, animate);
		}

		public Rect GetRectLayout(Rect offset, Rect source, bool animate)
		{
			return isActiveAndEnabled ? settings.GetRectLayout(offset, source, animate) : offset;
		}
	}
}
