using System;
using UnityEngine;

namespace UnityUtils.Common.Layout
{
	public class Contain1DLayout : ILayoutController
	{
		private enum Axis
		{
			Horizontal = 0,
			Vertical = 1
		}

		[SerializeField] private Axis axis;
		[SerializeField] private float spacing;

		public void Reload(Transform transform)
		{
			if (transform is not RectTransform rect)
				return;

			Vector3 _axis = GetAxisVector();
			float spacing = GetSpacing(rect, out int count);
			
			for (int i = 0; i < count; i++)
			{
				RectTransform child = transform.GetChild(i) as RectTransform;
				child.localPosition = i * spacing * _axis;
			}
		}

		private float GetSpacing(RectTransform transform, out int count)
		{
			count = transform.childCount;
			float max_size = axis switch
			{
				Axis.Horizontal => transform.rect.width,
				Axis.Vertical => transform.rect.height,
				_ => throw new ArgumentOutOfRangeException()
			};

			float m = spacing < 0 ? -1 : 1;
			float min_spacing = max_size / count;
			return Math.Min(min_spacing, Math.Abs(spacing)) * m;
		}

		private Vector3 GetAxisVector()
		{
			int _axis = (int)this.axis;
			return new Vector3(1 - _axis, _axis);
		}
	}
}
