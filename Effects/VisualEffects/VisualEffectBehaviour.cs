using System;
using System.Threading.Tasks;
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
		private IVisualEffectComponent[] components;

		public virtual void Play()
		{
			for (int i = 0; i < components.Length; i++)
				components[i].Play();
			IsPlaying = true;
		} 
		public virtual void Stop()
		{
			IsPlaying = false;
			for (int i = 0; i < components.Length; i++)
				components[i].Stop();
		}

		public async void OnEnd(Action action)
		{
			if (action == null)
				return;

			while (IsPlaying)
				await Task.Yield();
			action();
		}

		public async Task AwaitEnd()
		{
			while (IsPlaying)
				await Task.Yield();
		}

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

		public void ApplyOnComponents(IVisualEffectParameters parameters)
		{
			for (int i = 0; i < components.Length; i++)
				parameters.Apply(components[i]);
		}

		public void Dispose()
		{
			for (int i = 0; i < components.Length; i++)
				components[i].Dispose();
		}
		public virtual void Destroy()
		{
			Dispose();
			if (gameObject)
				Destroy(gameObject, 1);
		}
	}
}
