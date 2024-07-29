using System;
using UnityEngine;
using UnityUtils.Common.Layout;

namespace UnityUtils.Layouts.SpacingEvaluators
{
	[Serializable]
	public class DefinedSpacing : ISpacingEvaluator
	{
		[InspectorName("Minimum")]
		public Vector3 MinSpacing;
		[InspectorName("Maximum")]
		public Vector3 MaxSpacing;

		public Vector3 GetSpacing(int count, WorldGridLayout layout)
		{
			int max = layout.MaxElements;
			if (count <= max)
				return MaxSpacing;

			return Vector3.Lerp(MaxSpacing, MinSpacing, 0.9f * (count + 1 - max) / max);
		}
	}
}
