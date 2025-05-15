#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using System.Reflection;
using Utils.Delegates;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public abstract class InputProxy : IDisposable
	{
		private const BindingFlags methodReceiversFlags = BindingFlags.Public | BindingFlags.Instance | 
			BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly;

		public abstract bool IsActive { get; set; }

		protected readonly IInputReceiver receiver;
		protected readonly InputActionAsset asset;
		private readonly Dictionary<Guid, IActionInputInjector> injectors;

		protected InputProxy(IInputReceiver receiver, InputActionAsset asset)
		{
			injectors = new();
			this.receiver = receiver;
			this.asset = asset;
			receiver.ForeachMethodWithAttribute<InputReceiverAttribute>(ParseMethod, methodReceiversFlags);
		}

		private void ParseMethod(MethodInfo method, InputReceiverAttribute attribute)
		{
			InputActionMap map = asset.FindActionMap(attribute.map, true);
			InputAction action = map.FindAction(attribute.action);
			IActionInputInjector receiverInputInjector = CreateInjector(action, method);
			if (receiverInputInjector == null) return;

			injectors.Add(action.id, receiverInputInjector);
		}
		
		private IActionInputInjector CreateInjector(InputAction action, MethodInfo method)
		{
			return receiver.CreateInjector(action, method);
		}

		protected IEnumerable<IActionInputInjector> GetActions() => injectors.Values;
		protected bool TryGetAction(InputAction action, out IActionInputInjector actionInputInjector)
		{
			return injectors.TryGetValue(action.id, out actionInputInjector);
		}

		public abstract void Dispose();
	}
}
#endif
