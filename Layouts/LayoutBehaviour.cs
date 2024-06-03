using UnityEngine;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Common.Layout
{

	[ExecuteInEditMode]
	public class LayoutBehaviour : MonoBehaviour
	{
		[SerializeReference, Polymorphic]
		private ILayoutController controller;

		private void OnValidate() 
		{
			ReloadLayout();
		}
		private void OnEnable()
		{
			ReloadLayout();
		}
		private void OnTransformChildrenChanged()
		{
			ReloadLayout();
		}

		private void OnRectTransformDimensionsChange()
		{
			ReloadLayout();
		}

		public void ReloadLayout()
		{
			controller?.Reload(transform);
		}
	}
}
