using System;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.PropertyAttributes;
using Axis = UnityEngine.RectTransform.Axis;

namespace UnityUtils.DynamicScrollers
{
	public partial class DynamicScroller
	{
		[Serializable]
		public class ContentComponents
		{
			public enum SizingType
			{
				Additive,
				OnReload
			}

			public Vector2 Spacing => layoutGroup?.Spacing ?? Vector2.zero;

			public ILayoutGroupContainer Layout => layoutGroup;

			[SerializeReference, Polymorphic]
			private ILayoutGroupContainer layoutGroup;

			[field: SerializeField]
			public SizingType Sizing { get; private set; }

			public Vector2 StartPadding(Axis axis)
			{
				var padding = layoutGroup.Padding;
				return axis switch
				{
					Axis.Horizontal => new Vector2(padding.left, 0),
					Axis.Vertical => new Vector2(0, padding.top),
					_ => Vector2.zero
				};
			}

			public Vector2 EndPadding(Axis axis)
			{
				var padding = layoutGroup.Padding;
				return axis switch
				{
					Axis.Horizontal => new Vector2(padding.right, 0),
					Axis.Vertical => new Vector2(0, padding.bottom),
					_ => Vector2.zero
				};
			}
		}
	}
}
