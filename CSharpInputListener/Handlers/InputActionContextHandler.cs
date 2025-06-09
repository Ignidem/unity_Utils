#if ENABLE_INPUT_SYSTEM
using System;
using System.Reflection;
using UnityEngine.InputSystem;
using UnityUtils.CSharpInputListener.CallbackMiddleware;
using Utilities.Reflection;

namespace UnityUtils.CSharpInputListener
{
	public class InputActionContextHandler<T> : IActionInputHandler
		where T : struct
	{
		public InputAction Action { get; }
		public bool UsesPolling { get; }
		public delegate void Callback(IInputReceiver instance, InputAction.CallbackContext context, T value);
		private readonly Callback callback;
		private readonly ICallbackMiddleware<T> middleware;

		public InputActionContextHandler(InputAction action, MethodInfo method, InputReceiverAttribute attribute)
		{
			Action = action;
			UsesPolling = attribute.PollValue;
			this.callback = method.CreateInjectedDelegate<Callback>();
			if (attribute.MiddlewareType != null && attribute.MiddlewareType.Inherits(typeof(ICallbackMiddleware<T>)))
				middleware = (ICallbackMiddleware<T>)Activator.CreateInstance(attribute.MiddlewareType, args: new object[] { action });
		}

		public void Invoke(IInputReceiver instance)
		{
			if (middleware == null)
			{
				callback(instance, default, Action.ReadValue<T>());
				return;
			}
			
			if (middleware.InvokeActionValue(out T value))
				callback(instance, default, value);
		}

		public void Invoke(IInputReceiver instance, InputAction.CallbackContext input)
		{
			if (middleware == null)
			{
				callback(instance, input, input.ReadValue<T>());
				return;
			}
			
			if (middleware.InvokeContextValue(input, out T value))
				callback(instance, input, value);
		}
	}
}
#endif