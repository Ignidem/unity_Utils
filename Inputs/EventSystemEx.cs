using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityUtils.Inputs
{
	public static class EventSystemEx
	{
		public static void Select<T>(this T component, BaseEventData data = null)
			where T : Component, ISelectHandler
		{
			EventSystem current = EventSystem.current;
			if (!current) 
				return;

			current.SetSelectedGameObject(component.gameObject, data);
		}

		public static void Deselect(EventSystem system)
		{
			system.SetSelectedGameObject(null);
		}
	}
}
