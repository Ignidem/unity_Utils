#if ENABLE_INPUT_SYSTEM
using System;
using System.Reflection;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class ActionInputInjector<T> : IActionInputInjector
		where T : struct
	{
		public delegate void Callback(T input);
		public InputAction Action { get; }
		private readonly Callback callback;

		public ActionInputInjector(IInputReceiver receiver, InputAction action, MethodInfo method)
		{
			Action = action;
			this.callback = CreateDelegate(receiver, method);
		}

		private Callback CreateDelegate(IInputReceiver receiver, MethodInfo method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != 1) return null;
			
			return (Callback)Delegate.CreateDelegate(typeof(Callback), receiver, method);
		}
		
		public void Invoke(InputAction.CallbackContext input)
		{
			callback(input.action.ReadValue<T>());
		}
	}
}
#endif