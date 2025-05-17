#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public static class InputActionUtils
	{
		public static int GetBindingIndex(this InputAction.CallbackContext context)
		{
			return context.action.GetBindingIndexForControl(context.control);
		}

		public static InputBinding GetBinding(this InputAction.CallbackContext context)
		{
			int index = context.GetBindingIndex();
			return context.action.bindings[index];
		}
		
		public static bool IsContinuous(this InputAction.CallbackContext context)
		{
			return context.action.type == InputActionType.PassThrough;
		}
		
		public static bool IsModifier(this InputAction.CallbackContext context)
		{
			return context.GetBinding().IsModifier();
		}

		public static bool IsModifier(this InputBinding binding)
		{
			return binding is { isPartOfComposite: true, name: "modifier" };
		}
	}
}
#endif