using System;
using UnityEngine;
using UnityEngine.UI;
using Axis = UnityEngine.RectTransform.Axis;

namespace UnityUtils.DynamicScrollers
{
	public partial class DynamicScroller
	{
		[Serializable]
		public class ContentComponents
		{
			public float Spacing => layoutGroup ? layoutGroup.spacing : 0;

			[SerializeField] private HorizontalOrVerticalLayoutGroup layoutGroup;

			public Vector2 StartPadding(Axis axis)
			{
				return axis switch
				{
					Axis.Horizontal => new Vector2(layoutGroup ? layoutGroup.padding.left : 0, 0),
					Axis.Vertical => new Vector2(0, layoutGroup ? layoutGroup.padding.top : 0),
					_ => Vector2.zero
				};
			}

			public Vector2 EndPadding(Axis axis)
			{
				return axis switch
				{
					Axis.Horizontal => new Vector2(layoutGroup ? layoutGroup.padding.right : 0, 0),
					Axis.Vertical => new Vector2(0, layoutGroup ? layoutGroup.padding.bottom : 0),
					_ => Vector2.zero
				};
			}
		}
	}
}
