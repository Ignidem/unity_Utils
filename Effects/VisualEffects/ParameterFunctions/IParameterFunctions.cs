using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects.ParameterFunctions
{
	public interface IParameterFunctions<TComponent>
	{
		T GetValue<T>(TComponent component, int id);
		void SetValue<T>(TComponent component, int id, T value);
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
				return delegates.TryGetValue(id, out var value) ? value : null;
			}
			set
			{
				int id = Shader.PropertyToID(name);
				delegates[id] = value;
			}
		}

		private PropertyDelegates<TComponent, TValue> GetDelegate<TValue>(TComponent component, int id)
		{
			if (!delegates.TryGetValue(id, out IPropertyDelegates propertyDelegates))
			{
				return ThrowExceptions ? throw new MissingPropertyException(component, id, GetType()) : null;
			}

			if (propertyDelegates is not PropertyDelegates<TComponent, TValue> dgt)
			{
				return !ThrowExceptions ? null :
					throw new System.Exception($"Property Delegate {propertyDelegates.GetType()} is not of type {typeof(TValue)}");
			}

			return dgt;
		}

		public virtual T GetValue<T>(TComponent component, int id)
		{
			PropertyDelegates<TComponent, T> propDelegates = GetDelegate<T>(component, id);
			return propDelegates == null ? default : propDelegates.Get(component, id);
		}
		public virtual void SetValue<T>(TComponent component, int id, T value)
		{
			PropertyDelegates<TComponent, T> propDelegates = GetDelegate<T>(component, id);
			propDelegates?.Set(component, id, value);
		}
	}
}
