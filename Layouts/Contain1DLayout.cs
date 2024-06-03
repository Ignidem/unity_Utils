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
		[SerializeField] private float spacing_mult;

		public void Reload(Transform transform)
		{
			if (transform is not RectTransform rect)
				return;

			Vector3 _axis = GetAxisVector();
			float spacing = GetSpacing(rect, out int count);
			Vector3 offset = GetOffset(rect);
			
			for (int i = 0; i < count; i++)
			{
				RectTransform child = transform.GetChild(i) as RectTransform;
				child.localPosition = i * spacing * _axis;
			}
		}

		private Vector3 GetOffset(RectTransform rect)
		{
			return axis switch
			{
				Axis.Horizontal => new Vector3(rect.rect.width * rect.pivot.x, 0, 0),
				Axis.Vertical => new Vector3(0, rect.rect.height * rect.pivot.y, 0),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private float GetSpacing(RectTransform transform, out int count)
		{
			count = transform.childCount;
			(float max_size, float spacing) = axis switch
			{
				Axis.Horizontal => (transform.rect.width, transform.rect.height * spacing_mult),
				Axis.Vertical => (transform.rect.height, transform.rect.width * spacing_mult),
				_ => throw new ArgumentOutOfRangeException()
			};

			float m = spacing_mult < 0 ? -1 : 1;
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
