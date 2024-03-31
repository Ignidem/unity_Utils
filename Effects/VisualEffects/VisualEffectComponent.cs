using System;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class VisualEffectComponent : IVisualEffectComponents
	{
		[SerializeField]
		private VisualEffect vfx;
		public string Name => vfx.gameObject.name;

		public T GetValue<T>(int id)
		{
			return vfx.TryGetProperty(id, out T value) ? value : default;
		}

		public void SetValue<T>(int id, T value)
		{
			vfx.TrySetProperty(id, value);
		}
	}
}
