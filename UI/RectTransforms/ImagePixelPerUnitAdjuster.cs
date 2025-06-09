using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.UI
{
	public class ImagePixelPerUnitAdjuster : MonoBehaviour
	{
		[SerializeField] private Image image;
		[SerializeField] private RectTransform.Axis axis;
		[SerializeField] private float size;
		[SerializeField] private float multiplier;

		[ContextMenu("Copy Current Values")]
		public void CopyCurrentValues()
		{
			multiplier = image.pixelsPerUnitMultiplier;
			size = GetAxisSize();
		}
		
		public void AdjustPixelPerUnit()
		{
			if (!isActiveAndEnabled || !image || image.type != Image.Type.Sliced) 
				return;

			float currentSize = GetAxisSize();
			float ratio = size / currentSize;
			image.pixelsPerUnitMultiplier = multiplier * ratio;
		}

		private float GetAxisSize()
		{
			Rect rect = image.rectTransform.rect;
			return axis switch
			{
				RectTransform.Axis.Horizontal => rect.width,
				RectTransform.Axis.Vertical => rect.height,
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private void Awake()
		{
			AdjustPixelPerUnit();
		}

		private void OnEnable()
		{
			AdjustPixelPerUnit();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (!image)
			{
				image = GetComponent<Image>();
				CopyCurrentValues();
			}
			
			AdjustPixelPerUnit();
		}
		#endif

		private void OnRectTransformDimensionsChange()
		{
			AdjustPixelPerUnit();
		}
	}
}