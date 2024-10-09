using UnityEngine;

namespace UnityUtils.UI.RectTransforms
{
	[RequireComponent(typeof(RectTransform)), ExecuteInEditMode]
	public class Resizer : MonoBehaviour
	{
		[SerializeField] private RectTransform relative;
		[SerializeField] private Vector2 sizing;

		private void Awake()
		{
			Resize();
		}

		private void OnEnable()
		{
			Resize();
		}

		private void Resize()
		{
			if (transform is not RectTransform self || !relative)
				return;

			Vector2 size = relative.rect.size;
			self.sizeDelta = size * sizing;
		}
	}
}
