using System;
using Serialized;
using UnityEngine;
using UnityUtils.Effects.VisualEffects;
using UnityUtils.Events.UnityEvents;
using UnityUtils.PropertyAttributes;

namespace Assets.Game.Stats.Effects
{
    [Serializable]
    public class PropertyFunction: IKeyValuePair<int, IUnityEvent>
    {
        public int Key
        {
            get => property?.Hash ?? 0;
            set { }
        }

        [field: SerializeReference, Polymorphic] 
        private IComponentProperty property;
            
        [field: SerializeReference, Polymorphic]
        public IUnityEvent Value { get; set; }
    }
}