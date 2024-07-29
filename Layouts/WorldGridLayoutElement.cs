using UnityEngine;
using UnityUtils.Layouts.LayoutElementBehaviour;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Common.Layout
{
	public class WorldGridLayoutElement : MonoBehaviour, IWorldGridLayoutElement
	{
		[SerializeReference, Polymorphic]
		private ILayoutElementBehaviour[] behaviours;

		public Transform Transform => transform;

		public void SetLayoutPosition(LayoutElementInfo info)
		{
			for (int i = 0; i < behaviours.Length; i++)
			{
				ILayoutElementBehaviour item = behaviours[i];
				item?.UpdateElement(this, info);
			}
		}
	}
}
