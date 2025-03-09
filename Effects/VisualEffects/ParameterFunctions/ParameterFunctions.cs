using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Delegates;

namespace UnityUtils.Effects.VisualEffects.ParameterFunctions
{
    public abstract class ParameterFunctions<TComponent> : IParameterFunctions<TComponent>
    {
        private readonly Dictionary<int, IPropertyDelegate> delegates = new();

        protected virtual bool ThrowExceptions => true;
        protected virtual bool ConsiderAllOptional => false;

        protected IPropertyDelegate this[string name]
        {
            get
            {
                int id = Shader.PropertyToID(name);
                return this[id];
            }
            set
            {
                this[Shader.PropertyToID(name)] = value;
            }
        }
        protected IPropertyDelegate this[int id]
        {
            get
            {
                return delegates.TryGetValue(id, out var value) ? value : null;
            }
            set
            {
                delegates[id] = value;
            }
        }

        protected IdProperty<TComponent, TValue> GetDelegate<TValue>(int id, bool isOptional = false)
        {
            if (!delegates.TryGetValue(id, out IPropertyDelegate propertyDelegates))
                propertyDelegates = GetMissinDelegates<TValue>();

            if (propertyDelegates == null)
            {
                var exception = new Exception($"{GetType()} does not have delegates for {typeof(TValue)} {id}");
                OnException(isOptional, exception);
                return null;
            }

            if (propertyDelegates is not IdProperty<TComponent, TValue> dgt)
            {
                var exception = new Exception($"Property Delegate {propertyDelegates?.GetType()} is not of type {typeof(TValue)}");
                OnException(isOptional, exception);
                return null;
            }

            return dgt;
        }

        protected virtual IPropertyDelegate GetMissinDelegates<TValue>()
        {
            return null;
        }

        protected void OnException(bool isOptional, Exception exception)
        {
            if (isOptional || ConsiderAllOptional)
                return;

            if (ThrowExceptions)
                throw exception;

            Debug.LogException(exception);
        }

        public virtual T GetValue<T>(TComponent component, int id)
        {
            IdProperty<TComponent, T> propDelegates = GetDelegate<T>(id);
            return propDelegates == null ? default : propDelegates.Get(component, id);
        }
        public virtual void SetValue<T>(TComponent component, int id, T value, bool isOptional = false)
        {
            IdProperty<TComponent, T> propDelegates = GetDelegate<T>(id, isOptional);
            propDelegates?.Set(component, id, value);
        }
    }
}