using System;

namespace UnityUtils.CSharpInputListener
{
	public class InputReceiverAttribute : Attribute
	{
		public readonly string name;

		public InputReceiverAttribute(string name)
		{
			this.name = name;
		}
	}
}
