using UnityEngine;
using UnityUtils.Common.Layout;

namespace UnityUtils.DynamicScrollers
{
	[System.Serializable]
	public class LayoutControllerContainer : ILayoutGroupContainer
	{
		[SerializeField] private LayoutController controller;

		public Vector2 Spacing => default;
		public RectOffset Padding { get; } = new RectOffset();
		public Vector2Int GridSize => default;

		public Vector2 GetContentSize(RectTransform.Axis scrollAxis, RectTransform rect)
		{
			controller.ReloadLayout(false);
			return ((RectTransform)controller.transform).rect.size;
		}
	}
}
