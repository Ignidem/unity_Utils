#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class InputProxy<T> : IDisposable
		where T : IInputReceiver
	{
		protected readonly T receiver;
		protected ActionDelegateMap<T> delegateMap;

		private MonoBehaviour pollBehaviour;
		private Coroutine pollCoroutine;
		
		protected InputProxy(T receiver, ActionDelegateMap<T> map)
		{
			this.receiver = receiver;
			this.delegateMap = map;
		}

		public void UpdatePolling()
		{
			foreach (IActionInputHandler handler in delegateMap.GetHandlers())
			{
				if (handler.UsesPolling)
					handler.Invoke(receiver);
			}
		}

		public void SetPollingBehaviour(MonoBehaviour behaviour)
		{
			StopPollCoroutine();
			this.pollBehaviour = behaviour;
			StartPollCoroutine();
		}		
		private void StartPollCoroutine()
		{
			pollCoroutine = pollBehaviour.StartCoroutine(PollCoroutine());
		}
		private void StopPollCoroutine()
		{
			if (pollCoroutine == null) return;
			if (pollBehaviour)
				pollBehaviour.StopCoroutine(pollCoroutine);
				
			pollCoroutine = null;

		}
		private IEnumerator PollCoroutine()
		{
			WaitForEndOfFrame delay = new WaitForEndOfFrame();
			while (pollBehaviour)
			{
				UpdatePolling();
				yield return delay;
			}
		}
		
		public void OnAction(InputAction.CallbackContext context)
		{
			if (delegateMap.TryGetAction(context.action, out IActionInputHandler injector) && !injector.UsesPolling)
				injector.Invoke(receiver, context);
		}

		public void SetEnable(bool enabled)
		{
			if (enabled) Enable();
			else Disable();
		}

		public virtual void Enable()
		{
			StopPollCoroutine();
			StartPollCoroutine();
		}
		public virtual void Disable()
		{
			StopPollCoroutine();
		}

		public virtual void Dispose()
		{
			Disable();
		}
	}
}
#endif
