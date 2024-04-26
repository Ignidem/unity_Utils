using System;

namespace UnityUtils.Editor.ContextMenus
{
	public class PropertyContextMenuAttribute : Attribute
	{
		public readonly Type propertyType;

		public PropertyContextMenuAttribute(Type propertyType)
		{
			this.propertyType = propertyType;
		}
	}
}
