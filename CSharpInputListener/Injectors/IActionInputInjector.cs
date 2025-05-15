#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public interface IActionInputInjector
	{
		InputAction Action { get; }
		public void Invoke(InputAction.CallbackContext input);
	}
}
#endif