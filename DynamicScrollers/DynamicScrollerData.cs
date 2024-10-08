using UnityEngine;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.DynamicScrollers
{
	[RequireComponent(typeof(DynamicScroller))]
	public class DynamicScrollerData : MonoBehaviour
	{
		[SerializeField]
		private DynamicScroller scroller;

		[SerializeReference, Polymorphic]
		private IScrollerCellData[] data;

		private void Update()
		{
			if (scroller && data != null)
				scroller.Data = data;
			enabled = false;
		}
	}
}
