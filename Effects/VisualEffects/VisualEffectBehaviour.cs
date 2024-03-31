using UnityEngine;
using UnityEngine.Rendering;
using UnityUtils.PropertyAttributes;
using Utilities.Collections;

namespace UnityUtils.Effects.VisualEffects
{
	public class VisualEffectBehaviour : MonoBehaviour, IVisualEffect
	{
		public bool IsPlaying { get; private set; }

		public Transform Root => transform;

		[SerializeReference, Polymorphic]
		private IVisualEffectComponents[] components;

		public T GetValue<T>(string component, int id)
		{
			int index = components.IndexOf(c => c.Name == component);
			return components[index].GetValue<T>(id);
		}

		public void SetValue<T>(string component, int id, T value)
		{
			int index = components.IndexOf(c => c.Name == component);
			components[index].SetValue(id, value);
		}

		public void SetAll<T>(int id, T value)
		{
			for (int i = 0; i < components.Length; i++)
				components[i].SetValue(id, value);
		}

		public void Dispose()
		{
			for (int i = 0; i < components.Length; i++)
				components[i].Dispose();
		}
	}
}
