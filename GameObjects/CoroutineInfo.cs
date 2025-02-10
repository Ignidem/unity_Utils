using System.Collections;
using UnityEngine;

namespace UnityUtils.GameObjects
{
	public interface ICoroutineHandler
	{
		void Stop();
	}

	public readonly struct CoroutineInfo : ICoroutineHandler
	{
		public bool IsValid => behaviour != null && coroutine != null;

		public readonly MonoBehaviour behaviour;
		public readonly Coroutine coroutine;

		public CoroutineInfo(MonoBehaviour target, IEnumerator routine)
		{
			this.behaviour = target;
			coroutine = target.StartCoroutine(routine);
		}


		public void Stop()
		{
			if (IsValid)
				behaviour.StopCoroutine(coroutine);
		}
	}
}
