using System;
using UnityEngine;
using UnityUtils.Common.Layout;
using UnityUtils.RectUtils;
using Utils.Logger;

namespace UnityUtils.Layouts.RectLayout
{
	[Serializable]
	public class RectLayoutComponent : ILayoutComponent, IAnimatedRectLayoutElement
	{
		public bool IsEnabled => Transform.gameObject.activeSelf;
		public Vector2Int OffsetDirection => new Vector2Int(doWrapWidth ? 1 : 0, doWrapHeight ? 1 : 0);

		[field: SerializeField] public RectTransform Transform { get; private set; }
		[field: SerializeField] public RectTransform.Axis Axis { get; private set; }
		[SerializeField] public Vector2 spacing;

		[Header("Wrap Content")]
		[SerializeField]private bool doWrapWidth;
		[SerializeField]private bool doWrapHeight;
		[field: SerializeField] public float AnimationDuration { get; private set; }
		public float AnimationTime { get; private set; }
		public Rect TargetRect { get; private set; }

		private bool DoWrap => doWrapWidth || doWrapHeight;

		public void Reload(Transform sizeSource, bool animate)
		{
			if (sizeSource is not RectTransform parent)
				return;

			ReloadSize(default, parent.rect, animate);
		}
		private Rect ReloadSize(Rect rect, in Rect source, bool animate)
		{
			int count = Transform.childCount;
			for (int i = 0; i < count; i++)
			{
				Transform child = Transform.GetChild(i);
				if (!child.gameObject.activeSelf)
					continue;

				if (child.TryGetComponent(out IRectLayoutElement element))
				{
					if (!element.IsEnabled)
						continue;

					rect = element.GetRectLayout(rect, source, animate);
					//rect = rect.Wrap(offset);
				}
				else if (child is RectTransform rectChild)
				{
					rect = rect.Wrap(rectChild);
				}

				rect = rect.AddSize(spacing);
			}

			if (DoWrap)
				Wrap(rect.Wrap(default(Rect)), animate);

			return rect;
		}
		public Rect GetRectLayout(Rect offset, Rect source, bool animate)
		{
			return ReloadSize(offset, source, animate);
		}
		private void Wrap(Rect wrap, bool animate)
		{
			TargetRect = wrap;

			if (animate)
			{
				this.AnimateResizeAsync().LogException();
			}
			else
			{
				SetRect(TargetRect);
			}
		}
		public bool Next()
		{
			AnimationTime += Time.deltaTime; 
			bool hasNext = AnimationTime < AnimationDuration;
			if (!hasNext)
				AnimationTime = 0;

			return hasNext;
		}

		public void SetRect(Rect rect)
		{
			Vector2 delta = Transform.GetDeltaWithAnchors(rect, doWrapWidth, doWrapHeight);
			Transform.sizeDelta = delta;
		}
	}
}
