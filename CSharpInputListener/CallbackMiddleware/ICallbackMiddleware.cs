using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener.CallbackMiddleware
{
	public interface ICallbackMiddleware
	{
		
	}
	
	public interface ICallbackMiddleware<T> : ICallbackMiddleware
		where T : struct
	{
		bool InvokeActionValue(out T value);
		bool InvokeContextValue(InputAction.CallbackContext context, out T value);
	}
}