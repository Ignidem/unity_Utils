using UnityEngine;
using UnityUtils.Common.Layout;

namespace UnityUtils.Layouts.SpacingEvaluators
{
	public interface ISpacingEvaluator
	{
		Vector3 GetSpacing(int count, WorldGridLayout layout);
	}
}
