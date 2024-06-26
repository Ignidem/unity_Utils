using UnityEditor;
using UnityEngine;
using UnityUtils.Editor.ContextMenus;
using UnityUtils.Editor.SerializedProperties;

namespace UnityUtils.Effects.VisualEffects.Editor
{
	[PropertyContextMenu(typeof(IVisualEffectParameters))]
	public class ParametersPropertyContextMenu : IPropertyContextMenu
	{
		public void AddPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			if (!property.TryGetBoxedValue(out IVisualEffectParameters parameters))
				return;

			Transform transform = property.serializedObject.targetObject switch
			{
				GameObject obj => obj.transform,
				Component comp => comp.transform,
				_ => null
			};

			if (transform == null)
				return;

			IVisualComponent component = transform.GetComponentInChildren<IVisualComponent>();

			if (component == null)
				return;

			menu.AddItem(new GUIContent("Apply Parameters to Children"), false, () => ApplyToChildren(parameters, component));
		}

		private void ApplyToChildren(IVisualEffectParameters parameters, IVisualComponent component)
		{
			parameters.Apply(component);
			component.Play();
		}
	}
}
