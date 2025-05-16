#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public abstract class InputProxy<T> : IDisposable
		where T : IInputReceiver
	{
		protected virtual bool IsActive => receiver.IsActive && map.Asset.enabled;

		protected readonly T receiver;
		protected readonly ActionDelegateMap<T> map;
		private Dictionary<Guid, InputAction.CallbackContext> continuous;

		protected InputProxy(T receiver, ActionDelegateMap<T> map)
		{
			this.receiver = receiver;
			this.map = map;
			continuous = new();
		}

		public void Update()
		{
			if (!IsActive) return;
			
			foreach (KeyValuePair<Guid, InputAction.CallbackContext> action in continuous)
			{
				InvokeAction(action.Value);
			}
		}

		protected void OnAction(InputAction.CallbackContext context)
		{
			if (!map.TryGetAction(context.action, out IActionInputInjector injector))
				return;
			
			Phases phase = context.phase switch
			{
				InputActionPhase.Disabled => Phases.Disabled,
				InputActionPhase.Waiting => Phases.Waiting,
				InputActionPhase.Started => Phases.Started,
				InputActionPhase.Performed => Phases.Performed,
				InputActionPhase.Canceled => Phases.Canceled,
				_ => throw new ArgumentOutOfRangeException()
			};
			
			if (injector.Attribute.CallbackPhases.HasFlag(phase))
				InvokeAction(context);
			
			switch (phase)
			{
				case Phases.Started:
					StartContinuousAction(context);
					break;
				case Phases.Canceled or Phases.Disabled:
					StopContinuousAction(context);
					break;
			}
		}

		private void StartContinuousAction(InputAction.CallbackContext context)
		{
			if (map.TryGetAction(context.action, out IActionInputInjector injector) && 
			    injector.Attribute.CallbackPhases.HasFlag(Phases.Continuous))
				continuous[context.action.id] = context;
		}
		private void InvokeAction(InputAction.CallbackContext context)
		{
			if (map.TryGetAction(context.action, out IActionInputInjector injector))
				injector.Invoke(receiver, context);
		}		
		private void StopContinuousAction(InputAction.CallbackContext context)
		{
			continuous.Remove(context.action.id);
		}

		public void SetEnable(bool enabled)
		{
			if (enabled) Enable();
			else Disable();
		}
		public abstract void Enable();
		public abstract void Disable();

		public virtual void Dispose()
		{
			Disable();
		}
	}
}
#endif
