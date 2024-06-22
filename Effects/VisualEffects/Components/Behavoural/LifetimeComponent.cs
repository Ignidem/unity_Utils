using System;
using System.Collections;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class LifetimeComponent : IVisualEffectComponent,
		ISerializationCallbackReceiver
	{
		public enum Action
		{
			None,
			Destroy,
		}

		public string Name => nameof(LifetimeComponent);

		[SerializeField] private VisualEffectBehaviour subject;
		[SerializeField] private float lifetime;
		[SerializeField, HideInInspector] private bool destroy;
		[SerializeField] private Action action;

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
			switch (action)
			{
				case Action.Destroy:
					subject.Destroy();
					break;
			}
		}

		public void OnBeforeSerialize()
		{
			if (destroy)
				action = Action.Destroy;
		}

		public void OnAfterDeserialize() { }
	}
}
