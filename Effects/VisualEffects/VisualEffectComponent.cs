using System;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class VisualEffectComponent : IVisualEffectComponent
	{
		[SerializeField]
		private VisualEffect vfx;
		public string Name => vfx.gameObject.name;

		public void Play()
		{
			vfx.gameObject.SetActive(true);
			vfx.Play();
		}

		public void Stop()
		{
			vfx.Stop();
			vfx.gameObject.SetActive(false);
		}

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
