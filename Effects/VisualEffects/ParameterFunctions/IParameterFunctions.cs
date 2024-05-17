using System;
using System.Collections.Generic;
using UnityEngine;
using WarmongersAPI.Systems;

namespace UnityUtils.Effects.VisualEffects.ParameterFunctions
{
	public interface IParameterFunctions<TComponent>
	{
		T GetValue<T>(TComponent component, int id);
		void SetValue<T>(TComponent component, int id, T value, bool isOptional = false);
	}

	public abstract class ParameterFunctions<TComponent> : IParameterFunctions<TComponent>
	{
		private readonly Dictionary<int, IPropertyDelegates> delegates = new();

		protected virtual bool ThrowExceptions => true;

		protected IPropertyDelegates this[string name]
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
		protected IPropertyDelegates this[int id]
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

		protected PropertyDelegates<TComponent, TValue> GetDelegate<TValue>(TComponent component, int id, bool isOptional = false)
		{
			if (!delegates.TryGetValue(id, out IPropertyDelegates propertyDelegates))
			{
				var exception = new MissingPropertyException(component, id, GetType());
				OnException(isOptional, exception);
				return null;
			}

			if (propertyDelegates is not PropertyDelegates<TComponent, TValue> dgt)
			{
				var exception = new Exception($"Property Delegate {propertyDelegates.GetType()} is not of type {typeof(TValue)}");
				OnException(isOptional, exception);
				return null;
			}

			return dgt;
		}

		protected void OnException(bool isOptional, Exception exception)
		{
			if (ThrowExceptions && !isOptional)
				throw exception;

			if (!isOptional)
				exception.LogException();
		}

		public virtual T GetValue<T>(TComponent component, int id)
		{
			PropertyDelegates<TComponent, T> propDelegates = GetDelegate<T>(component, id);
			return propDelegates == null ? default : propDelegates.Get(component, id);
		}
		public virtual void SetValue<T>(TComponent component, int id, T value, bool isOptional = false)
		{
			PropertyDelegates<TComponent, T> propDelegates = GetDelegate<T>(component, id, isOptional);
			propDelegates?.Set(component, id, value);
		}
	}
}
