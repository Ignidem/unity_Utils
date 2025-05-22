#if ENABLE_INPUT_SYSTEM
using System.Reflection;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public delegate IActionInputHandler Constructor(InputAction action, MethodInfo method, InputReceiverAttribute attribute);
}
#endif