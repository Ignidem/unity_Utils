#if ENABLE_INPUT_SYSTEM
using System.Reflection;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public delegate IActionInputInjector Constructor(IInputReceiver receiver, InputAction action, MethodInfo method);
}
#endif