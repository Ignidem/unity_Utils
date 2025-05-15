#if ENABLE_INPUT_SYSTEM
using System;
using System.Reflection;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class ActionContextInjector<T> : IActionInputInjector
		where T : struct
	{
		public delegate void Callback(InputAction.CallbackContext context, T value);
		public InputAction Action { get; }
		private readonly Callback callback;

		public ActionContextInjector(IInputReceiver receiver, InputAction action, MethodInfo method)
		{
			Action = action;
			this.callback = CreateDelegate(receiver, method);
		}

		private Callback CreateDelegate(IInputReceiver receiver, MethodInfo method)
		{
			return (Callback)Delegate.CreateDelegate(typeof(Callback), receiver, method);
		}
		
		public void Invoke(InputAction.CallbackContext input)
		{
			callback(input, input.ReadValue<T>());
		}
	}
}
#endif