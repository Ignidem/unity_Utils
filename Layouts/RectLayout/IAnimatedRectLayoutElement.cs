using UnityEngine;

namespace UnityUtils.Layouts.RectLayout
{
	public interface IAnimatedRectLayoutElement : IRectLayoutElement
	{
		RectTransform.Axis Axis { get; }
		float AnimationTime { get; }
		float AnimationDuration { get; }
		Rect TargetRect { get; }

		bool Next();
		void SetRect(Rect rect);
	}
}
