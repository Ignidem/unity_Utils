#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public abstract class InputProxy<T> : IDisposable
		where T : IInputReceiver
	{
		private class CoroutineInfo
		{
			public InputAction.CallbackContext lastContext;
			
			private Coroutine coroutine;
			
		}
		
		protected virtual bool IsActive => receiver.IsActive && map.Asset.enabled;

		protected readonly T receiver;
		protected readonly ActionDelegateMap<T> map;
		
		//per binding and not per action;
		private readonly Dictionary<Guid, InputAction.CallbackContext> polls = new();

		protected InputProxy(T receiver, ActionDelegateMap<T> map)
		{
			this.receiver = receiver;
			this.map = map;
		}

		public void UpdateContinuous()
		{
			foreach (InputAction.CallbackContext context in polls.Values)
			{
				if (map.TryGetAction(context.action, out IActionInputInjector injector))
					injector.Invoke(receiver, context);
			}
		}
		
		protected void OnAction(InputAction.CallbackContext context)
		{
			if (!map.TryGetAction(context.action, out IActionInputInjector injector))
				return;

			InputBinding binding = context.GetBinding();
			if (binding.IsModifier()) return;

			switch (context.phase)
			{
				case InputActionPhase.Started:
					if (!context.IsContinuous())
						polls[binding.id] = context;
					break;
				case InputActionPhase.Performed:
					injector.Invoke(receiver, context);
					break;
				case InputActionPhase.Canceled or InputActionPhase.Disabled:
					if (!context.IsContinuous())
						polls.Remove(binding.id);
					break;
			}
		}


		public void SetEnable(bool enabled)
		{
			if (enabled) Enable();
			else Disable();
		}

		public virtual void Enable()
		{
			
		}
		public abstract void Disable();

		public virtual void Dispose()
		{
			Disable();
		}
	}
}
#endif
