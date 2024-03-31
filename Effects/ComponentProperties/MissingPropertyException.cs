using System;

namespace UnityUtils.Effects
{
	public class MissingPropertyException : Exception
	{
		private const string messageFormat = "{0} {1} does not have property {2} of type {3}";

		public readonly UnityEngine.Object component;
		public readonly int id;

		public MissingPropertyException(UnityEngine.Object component, int id, Type type)
			: base(string.Format(messageFormat, component.GetType().Name, component.name, id, type.Name))
		{
			this.component = component;
			this.id = id;
		}
	}
}
