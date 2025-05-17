#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

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
		private readonly Dictionary<Guid, InputAction.CallbackContext> continuousContext = new();

		protected InputProxy(T receiver, ActionDelegateMap<T> map)
		{
			this.receiver = receiver;
			this.map = map;
		}

		public void UpdateContinuous()
		{
			foreach (InputAction.CallbackContext context in continuousContext.Values)
			{
				InvokeAction(context);
			}
		}
		
		protected void OnAction(InputAction.CallbackContext context)
		{
			if (!map.TryGetAction(context.action, out _))
				return;

			InputBinding binding = context.GetBinding();
			if (binding.IsModifier()) return;
			
			//Is handled through continuous
			if (context.IsContinuous())
			{
				continuousContext[binding.id] = context;
				return;
			}
			
			InvokeAction(context);
		}

		private void InvokeAction(InputAction.CallbackContext context)
		{
			if (map.TryGetAction(context.action, out IActionInputInjector injector))
				injector.Invoke(receiver, context);
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
