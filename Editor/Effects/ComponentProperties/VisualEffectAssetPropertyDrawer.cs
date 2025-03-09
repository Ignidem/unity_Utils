#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.VFX;

namespace UnityUtils.Effects.VisualEffects
{
    [CustomPropertyDrawer(typeof(VisualEffectAssetProperty))]
    public class VisualEffectAssetPropertyDrawer : ComponentPropertyDrawer<VisualEffectAsset>
    {
        List<VFXExposedProperty> props = new ();

        protected override void UpdatePropertyList(string selectedName)
        {
            props.Clear();
            component.GetExposedProperties(props);
            names = props.Select(p => p.name).ToArray();
            selected = Array.IndexOf(names, selectedName);
        }
    }
}
#endif