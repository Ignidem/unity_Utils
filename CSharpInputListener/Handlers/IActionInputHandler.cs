#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public interface IActionInputHandler
	{
		InputAction Action { get; }
		bool UsesPolling { get; }
		void Invoke(IInputReceiver receiver);
		void Invoke(IInputReceiver receiver, InputAction.CallbackContext input);
	}
}
#endif