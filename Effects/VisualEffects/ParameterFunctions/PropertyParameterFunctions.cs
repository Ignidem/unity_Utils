using Assets.Game.Stats.Effects;
using UnityEngine.Events;
using UnityUtils.Events.UnityEvents;

namespace UnityUtils.Effects.VisualEffects.ParameterFunctions
{
    public class PropertyParameterFunctions<TComponent> : PropertyFunctions, IParameterFunctions<TComponent>
    {
        public T GetValue<T>(TComponent component, int id)
        {
            return default;
        }

        public void SetValue<T>(TComponent component, int id, T value, bool isOptional = false)
        {
            if (!TryGetValue(id, out IUnityEvent evnt) || evnt is not UnityEvent<T> unityEvent)
                return;
            
            unityEvent.Invoke(value);
        }
    }
}