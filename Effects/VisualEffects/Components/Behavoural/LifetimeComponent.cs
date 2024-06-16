using System;
using System.Collections;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class LifetimeComponent : IVisualEffectComponent
	{
		public string Name => nameof(LifetimeComponent);

		[SerializeField] private VisualEffectBehaviour subject;
		[SerializeField] private float lifetime;
		[SerializeField] private bool destroy;

		private Coroutine lifetimeCoroutine;

		public T GetValue<T>(int id) => default;
		public void SetValue<T>(int id, T value, bool isOptional = false) { }

		public void Play()
		{
			if (lifetimeCoroutine != null)
				return;

			lifetimeCoroutine = subject.StartCoroutine(CountdownLifetime());
		}

		public void Stop()
		{
			if (lifetimeCoroutine != null)
				subject.StopCoroutine(lifetimeCoroutine);
		}

		private IEnumerator CountdownLifetime()
		{
			yield return new WaitForSeconds(lifetime);
			subject.Stop();
			if (destroy)
				subject.Destroy();
		}
	}
}
