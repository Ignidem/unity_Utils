using UnityEngine;

namespace UnityUtils.PropertyAttributes
{
	public class ReadOnlyAttribute : PropertyAttribute 
	{
		public readonly bool hasToggle;
		public ReadOnlyAttribute(bool hasToggle = false) 
		{
			this.hasToggle = hasToggle;
		}
	}
}
