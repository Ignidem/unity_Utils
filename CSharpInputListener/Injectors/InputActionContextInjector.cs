#if ENABLE_INPUT_SYSTEM
using System;
using System.Reflection;
using UnityEngine.InputSystem;
using Utilities.Reflection;

namespace UnityUtils.CSharpInputListener
{
	public class InputActionContextInjector<T> : IActionInputInjector
		where T : struct
	{
		public InputAction Action { get; }
		public delegate void Callback(IInputReceiver instance, InputAction.CallbackContext context, T value);
		private readonly Callback callback;

		public InputActionContextInjector(InputAction action, MethodInfo method)
		{
			Action = action;
			this.callback = method.CreateInjectedDelegate<Callback>();
		}

		public void OnUpdate(IInputReceiver instance)
		{
			callback(instance, default, Action.ReadValue<T>());
		}

		public void Invoke(IInputReceiver instance, InputAction.CallbackContext input)
		{
			callback(instance, input, input.ReadValue<T>());
		}
	}
}
#endif