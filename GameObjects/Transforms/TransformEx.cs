using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.Transforms
{
	public static class TransformEx
	{
		public static IEnumerable<Transform> GetChildrenRecursive(this Transform transform)
		{
			foreach (Transform child in transform)
			{
				yield return child;

				foreach (Transform subChild in child.GetChildrenRecursive())
				{
					yield return subChild;
				}
			}
		}

		public static void ClearChildren(this Transform transform)
		{
			foreach (Transform child in transform)
			{
				if (child)
					Object.Destroy(child.gameObject);
			}
		}
	}
}
