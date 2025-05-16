using System;

namespace UnityUtils.CSharpInputListener
{
	[Flags]
	public enum Phases
	{
		Disabled = 1,
		Waiting = 1<<1,
		Started = 1<<2,
		Performed = 1<<3,
		Continuous = 1<<4,
		Canceled = 1<<5,
	}
}