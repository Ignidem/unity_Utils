#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.InputSystem;
using Utils.Delegates;

namespace UnityUtils.CSharpInputListener
{
	public interface IDelegateBinder
	{
		InputActionAsset Asset { get; }
		IEnumerable<IActionInputHandler> GetHandlers();
		bool TryGetAction(InputAction action, out IActionInputHandler actionInputHandler);
	}
	
	public class ActionDelegateMap<T> : IDelegateBinder
	{
		private const BindingFlags methodReceiversFlags = BindingFlags.Public | BindingFlags.Instance | 
		                                                  BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly;
		public InputActionAsset Asset { get; }
		private readonly Dictionary<Guid, IActionInputHandler> injectors;

		public ActionDelegateMap(InputActionAsset asset)
		{
			this.Asset = asset;
			injectors = new();
			Type type = typeof(T);
			type.ForeachMethodWithAttributes<InputReceiverAttribute>(ParseMethod, methodReceiversFlags);
		}
		
		private void ParseMethod(MethodInfo method, InputReceiverAttribute attribute)
		{
			InputActionMap map = Asset.FindActionMap(attribute.map, true);
			InputAction action = map.FindAction(attribute.action, true);
			Constructor constructor = action.GetInjectorConstructor();
			IActionInputHandler receiverInputHandler = constructor?.Invoke(action, method, attribute);
			if (receiverInputHandler == null) return;

			injectors.Add(action.id, receiverInputHandler);
		}
		
		public IEnumerable<IActionInputHandler> GetHandlers() => injectors.Values;

		public bool TryGetAction(InputAction action, out IActionInputHandler actionInputHandler)
		{
			return TryGetAction(action.id, out actionInputHandler);
		}
		public bool TryGetAction(Guid actionId, out IActionInputHandler actionInputHandler)
		{
			return injectors.TryGetValue(actionId, out actionInputHandler);
		}
	}
}
#endif