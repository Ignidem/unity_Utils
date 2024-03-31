using System;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IVisualEffectComponents : IDisposable
	{
		string Name { get; }

		T GetValue<T>(int id);
		void SetValue<T>(int id, T value);

		void IDisposable.Dispose() { }
	}
}
