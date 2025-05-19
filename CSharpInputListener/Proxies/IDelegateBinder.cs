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
		IEnumerable<IActionInputInjector> GetActions();
		bool TryGetAction(InputAction action, out IActionInputInjector actionInputInjector);
	}
	
	public class ActionDelegateMap<T> : IDelegateBinder
	{
		private const BindingFlags methodReceiversFlags = BindingFlags.Public | BindingFlags.Instance | 
		                                                  BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly;
		public InputActionAsset Asset { get; }
		private readonly Dictionary<Guid, IActionInputInjector> injectors;

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
			IActionInputInjector receiverInputInjector = constructor?.Invoke(action, method);
			if (receiverInputInjector == null) return;

			injectors.Add(action.id, receiverInputInjector);
		}
		
		public IEnumerable<IActionInputInjector> GetActions() => injectors.Values;

		public bool TryGetAction(InputAction action, out IActionInputInjector actionInputInjector)
		{
			return TryGetAction(action.id, out actionInputInjector);
		}
		public bool TryGetAction(Guid actionId, out IActionInputInjector actionInputInjector)
		{
			return injectors.TryGetValue(actionId, out actionInputInjector);
		}
	}
}
#endif