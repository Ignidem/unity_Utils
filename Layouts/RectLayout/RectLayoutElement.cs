using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.RectUtils;
using UnityUtils.UI.Selectable;
using Utils.Logger;

namespace UnityUtils.Layouts.RectLayout
{
	public abstract class RectLayoutElement : MonoBehaviour, IAnimatedRectLayoutElement
	{
		public bool IsEnabled => enabled;
		public RectTransform Transform => (RectTransform)transform;
		[field: SerializeField] public RectTransform.Axis Axis { get; private set; }
		[field: SerializeField] public Vector2Int OffsetDirection { get; private set; }

		[field: Header("Animation")]
		[field: SerializeField] public float AnimationDuration { get; private set; }
		public float AnimationTime { get; private set; }
		public Rect TargetRect { get; private set; }

		protected virtual void OnEnable() { }
		protected virtual void OnDisable() { }

		public void Toggle(Button button)
		{
			OnSetEnabled(enabled = !button.IsOn);
		}

		protected virtual void OnSetEnabled(bool enabled) { }

		[ContextMenu(nameof(ParentLayout))]
		public void ParentLayout()
		{
			Rect source = ((RectTransform)transform.parent).rect;
			GetRectLayout(default, source, false);
		}

		public Rect GetRectLayout(Rect offset, Rect source, bool animate)
		{
			if (!isActiveAndEnabled)
				return offset;

			TargetRect = Axis switch
			{
				RectTransform.Axis.Horizontal => GetHorizontalRectLayout(offset, source),
				RectTransform.Axis.Vertical => GetVerticalRectLayout(offset, source),
				_ => throw new ArgumentOutOfRangeException(nameof(Axis)),
			};

			if (animate)
			{
				this.AnimateResizeAsync().LogException();
			}
			else
			{
				SetRect(TargetRect);
			}

			return TargetRect;
		}

		protected abstract Rect GetVerticalRectLayout(Rect offset, Rect source);
		protected abstract Rect GetHorizontalRectLayout(Rect offset, Rect source);

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
			RectTransform transform = Transform;
			Vector2 localPos = this.GetLocalPosition(rect.position);
			transform.localPosition = localPos;
			transform.sizeDelta = this.GetDeltaSize(rect.size);
		}
	}
}
