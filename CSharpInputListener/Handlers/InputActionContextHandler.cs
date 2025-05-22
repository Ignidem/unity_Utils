#if ENABLE_INPUT_SYSTEM
using System.Reflection;
using UnityEngine.InputSystem;
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

		public InputActionContextHandler(InputAction action, MethodInfo method, InputReceiverAttribute attribute)
		{
			Action = action;
			UsesPolling = attribute.PollValue;
			this.callback = method.CreateInjectedDelegate<Callback>();
		}

		public void Invoke(IInputReceiver instance)
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