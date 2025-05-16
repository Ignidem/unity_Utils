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
		public InputReceiverAttribute Attribute { get; }
		public delegate void Callback(IInputReceiver instance, InputAction.CallbackContext context, T value);
		private readonly Callback callback;

		public InputActionContextInjector(InputReceiverAttribute attribute, MethodInfo method)
		{
			Attribute = attribute;
			this.callback = method.CreateInjectedDelegate<Callback>();
		}
		
		public void Invoke(IInputReceiver instance, InputAction.CallbackContext input)
		{
			callback(instance, input, input.ReadValue<T>());
		}
	}
}
#endif