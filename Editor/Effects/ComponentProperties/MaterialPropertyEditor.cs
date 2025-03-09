#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using Utilities.Collections;
using System;

namespace UnityUtils.Effects.VisualEffects
{
	[CustomPropertyDrawer(typeof(MaterialProperty))]
	public class MaterialPropertyDrawer : ComponentPropertyDrawer<Material>
	{
		UnityEditor.MaterialProperty[] props;

		protected override void UpdatePropertyList(string selectedName)
		{
			props = MaterialEditor.GetMaterialProperties(new UnityEngine.Object[] { component });
			names = props.Select(p => p.name).ToArray();
			selected = Array.IndexOf(names, selectedName);
		}
	}
}
#endif