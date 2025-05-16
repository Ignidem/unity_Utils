#if ENABLE_INPUT_SYSTEM
using System;
using System.Reflection;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class CallbackInfo
	{
		public readonly InputAction action;
		public readonly MethodInfo method;

		public readonly ParameterInfo[] parameters;
		public readonly int inputIndex;
		public Type InputType => parameters[inputIndex].ParameterType;

		public CallbackInfo(InputAction action, MethodInfo method)
		{
			this.action = action;
			this.method = method;
			
			parameters = method.GetParameters();
		}
	}
}
#endif