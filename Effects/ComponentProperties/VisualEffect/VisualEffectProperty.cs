using System;
using UnityEngine.VFX;

namespace UnityUtils.Effects.VisualEffects
{
    [Serializable]
    public class VisualEffectProperty : ComponentProperty<VisualEffect>
    {
        public override void Set<TValue>(VisualEffect component, TValue value)
        {
            component.TrySetProperty(Hash, value, false);
        }
    }
    
    [Serializable]
    public class VisualEffectAssetProperty : ComponentProperty<VisualEffectAsset>
    {
        public override void Set<TValue>(VisualEffectAsset component, TValue value)
        {
           
        }
    }
}