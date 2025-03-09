using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
    public interface IComponentProperty
    {
        int Hash { get; }
    }
    
    [Serializable]
    public abstract class ComponentProperty<T> : IComponentProperty
    {
        [SerializeField] protected T component;
        [SerializeField] private string propertyName;
        [SerializeField, HideInInspector] private int propertyHash;
        public int Hash => propertyHash;
		
        public void Set<TValue>(TValue value) => Set(component, value);
        public abstract void Set<TValue>(T component, TValue value);
    }
}