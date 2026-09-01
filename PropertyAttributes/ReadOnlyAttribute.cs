using UnityEngine;

namespace UnityUtils.PropertyAttributes
{
	public class ReadOnlyAttribute : PropertyAttribute 
	{
		public bool HasToggle { get; set; }
		public bool AsLabel { get; set; }
		
		public ReadOnlyAttribute(bool hasToggle = false) 
		{
			this.HasToggle = hasToggle;
		}
	}
}
