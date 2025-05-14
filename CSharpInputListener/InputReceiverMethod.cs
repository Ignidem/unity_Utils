#if ENABLE_INPUT_SYSTEM
using System.Reflection;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public interface IInputReceiverMethod
	{
		public void Invoke(IInputReceiver receiver, InputAction input);
	}

	public class InputReceiverMethod<T> : IInputReceiverMethod
		where T : struct
	{
		private readonly MethodInfo method;

		public InputReceiverMethod(MethodInfo method)
		{
			this.method = method;
		}

		public void Invoke(IInputReceiver receiver, InputAction input)
		{
			//TODO! Use Compiled Delegate
			method.Invoke(receiver, new object[] { input.ReadValue<T>() });
		}
	}
}
#endif