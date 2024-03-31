using System;
using System.Collections.Generic;

namespace UnityUtils.Effects
{
	public static class PropertyDelegatesUtils
	{
		public static PropertyDelegates<TComponent, TValue> GetDelegates<TComponent, TValue>(
			this Dictionary<Type, IPropertyDelegates> delegates)
			where TComponent : UnityEngine.Object
		{
			if (!delegates.TryGetValue(typeof(TValue), out IPropertyDelegates prop))
			{
				throw new Exception($"PropertyDelegates for {typeof(TComponent).Name} {typeof(TValue).Name} was not defined.");
			}

			if (prop is not PropertyDelegates<TComponent, TValue> propDelegates)
			{
				throw new Exception($"PropertyDelegates for {typeof(TComponent).Name} {typeof(TValue).Name} is not the expected type.");
			}

			return propDelegates;
		}
	}
}
