using System;

namespace UnityUtils.CSharpInputListener
{
	public class InputReceiverAttribute : Attribute
	{
		public readonly string map;
		public readonly string action;

		public InputReceiverAttribute(string map, string action)
		{
			this.map = map;
			this.action = action;
		}
	}
}
