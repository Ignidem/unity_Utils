#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public interface IActionInputInjector
	{
		public InputReceiverAttribute Attribute { get; }
		public void Invoke(IInputReceiver instance, InputAction.CallbackContext input);
	}
}
#endif