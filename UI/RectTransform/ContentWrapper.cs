using UnityEngine;

namespace UnityUtils.UI.RectTransforms
{
	[RequireComponent(typeof(RectTransform)), ExecuteInEditMode]
	public class ContentWrapper : MonoBehaviour
	{
		[SerializeField]
		private bool shouldUpdate;

		private void Update()
		{
			if (shouldUpdate)
				Resize();
		}

		private void OnTransformChildrenChanged()
		{
			shouldUpdate = true;
		}

		public void Resize()
		{
			shouldUpdate = false;
			Rect reach = default;
			int count = transform.childCount;

			for (int i = 0; i < count; i++)
			{
				Transform child = transform.GetChild(i);
				if (child.gameObject.activeInHierarchy && child is RectTransform childTransform)
					reach = ExpandRect(reach, childTransform);
			}

			SetRect(reach);
		}

		private Rect ExpandRect(Rect origin, RectTransform child)
		{
			Rect target = child.rect;
			Vector2 pos = child.localPosition;
			return Rect.MinMaxRect(
				Mathf.Min(origin.xMin, pos.x + target.xMin),
				Mathf.Min(origin.yMin, pos.y + target.yMin),
				Mathf.Max(origin.xMax, pos.x + target.xMax),
				Mathf.Max(origin.yMax, pos.y + target.yMax)
				);
		}

		private void SetRect(Rect reach)
		{
			RectTransform rectTransform = transform as RectTransform;
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, reach.width);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, reach.height);
		}
	}
}
