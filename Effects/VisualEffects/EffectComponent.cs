using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	public abstract class EffectComponent : MonoBehaviour, IVisualEffectComponent
	{
		public string Name => name;

		public abstract void Play();
		public abstract void Stop();

		public abstract T GetValue<T>(int id);
		public abstract void SetValue<T>(int id, T value, bool isOptional = false);

		public virtual void Dispose() { }
	}
}
