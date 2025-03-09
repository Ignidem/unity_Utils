using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class ChildBehaviour : IVisualEffectComponent
	{
		[SerializeField] private VisualEffectBehaviour child;

		public string Name => child.name;

		public T GetValue<T>(int id)
		{
			return child.GetValue<T>("Any", id);
		}

		public void Play()
		{
			child.Play();
		}

		public void SetValue<T>(int id, T value, bool isOptional = false)
		{
			child.SetAll(id, value, isOptional);
		}

		public void Stop()
		{
			child.Stop();
		}
	}
}
