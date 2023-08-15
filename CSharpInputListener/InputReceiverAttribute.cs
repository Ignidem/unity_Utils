using System;

namespace Assets.External.unity_utils.CSharpInputListener
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
