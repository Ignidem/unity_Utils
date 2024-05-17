using System;

namespace UnityUtils.Effects
{
	public delegate bool HasPropertyDelegate<TComponent>(TComponent comp, int id);
	public delegate TValue GetPropertyDelegate<TComponent, TValue>(TComponent comp, int id);
	public delegate void SetPropertyDelegate<TComponent, TValue>(TComponent comp, int id, TValue value);

	public class PropertyDelegates<TComponent, TValue> : IPropertyDelegates
	{
		public static readonly PropertyDelegates<TComponent, TValue> Empty = 
			new PropertyDelegates<TComponent, TValue>((_, _) => true, (_, _) => default, (_, _, _) => { });

		private readonly HasPropertyDelegate<TComponent> hasProperty;
		private readonly GetPropertyDelegate<TComponent, TValue> getProperty;
		private readonly SetPropertyDelegate<TComponent, TValue> setProperty;

		public PropertyDelegates(
			HasPropertyDelegate<TComponent> hasDelegate,
			GetPropertyDelegate<TComponent, TValue> getDelegate,
			SetPropertyDelegate<TComponent, TValue> setDelegate
			)
		{
			hasProperty = hasDelegate;
			getProperty = getDelegate;
			setProperty = setDelegate;
		}

		public bool Has(TComponent component, int id)
		{
			return hasProperty(component, id);
		}

		public TValue Get(TComponent component, int id)
		{
			if (!Has(component, id))
				throw GetException(component, id);

			return getProperty(component, id);
		}

		public bool TryGet(TComponent component, int id, out TValue value)
		{
			if (!Has(component, id))
			{
				value = default;
				return false;
			}

			value = getProperty(component, id);
			return true;
		}

		public void Set(TComponent component, int id, TValue value)
		{
			if (!Has(component, id))
				throw GetException(component, id);

			setProperty(component, id, value);
		}

		public bool TrySet(TComponent component, int id, TValue value)
		{
			if (!Has(component, id))
				return false;

			setProperty(component, id, value);
			return true;
		}

		private MissingPropertyException GetException(TComponent component, int id)
		{
			return new MissingPropertyException(component, id, typeof(TValue));
		}
	}
}
