using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.RectUtils;

namespace UnityUtils.Layouts.RectLayout
{
	public static class RectLayoutUtils
	{
		public static async Task AnimateResizeAsync(this IAnimatedRectLayoutElement element)
		{
			if (System.Math.Abs(element.AnimationDuration) > 0)
			{
				element.SetRect(element.TargetRect);
				return;
			}
			
			if (element.AnimationTime > 0)
				return;

			RectTransform transform = element.Transform;
			RectTransform.Axis axis = element.Axis;

			Vector2 startPos = transform.position;
			Vector2 startSize = transform.sizeDelta;

			while (element.Next())
			{
				float norm = element.AnimationTime / element.AnimationDuration;
				if (!transform)
					break;

				Vector2 pos = Vector2.Lerp(startPos, element.TargetRect.position, norm); 
				element.SetRect(new Rect(pos, Vector2.Lerp(startSize, element.TargetRect.size, norm)));
				await Task.Yield();
			}

			element.SetRect(element.TargetRect);
		}
		
		public static Vector2 GetLocalPosition(this IRectLayoutElement element, Vector2 target)
		{
			return new Vector2(
				GetLocalPosition(element, target, RectTransform.Axis.Horizontal), 
				GetLocalPosition(element, target, RectTransform.Axis.Vertical)
				);
		}
		public static float GetLocalPosition(this IRectLayoutElement element, Vector2 target, RectTransform.Axis axis)
		{
			RectTransform transform = element.Transform;
			int n = (int)axis;
			float offset = element.OffsetDirection[n];
			return offset == 0 ? transform.localPosition[n] : target[n] * offset;
		}

		public static Vector2 GetDeltaSize(this IRectLayoutElement element, Vector2 target)
		{
			RectTransform transform = element.Transform;
			return new Vector2(
				GetDeltaSize(transform, target, RectTransform.Axis.Horizontal), 
				GetDeltaSize(transform, target, RectTransform.Axis.Vertical)
				);
		}
		public static float GetDeltaSize(this RectTransform element, Vector2 target, RectTransform.Axis axis)
		{
			return element.IsStretched(axis) ? 0 : target[(int)axis];
		}
	}
}
