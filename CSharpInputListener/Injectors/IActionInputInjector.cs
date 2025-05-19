#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public interface IActionInputInjector
	{
		InputAction Action { get; }
		void OnUpdate(IInputReceiver instance);
		void Invoke(IInputReceiver instance, InputAction.CallbackContext input);
	}
}
#endif