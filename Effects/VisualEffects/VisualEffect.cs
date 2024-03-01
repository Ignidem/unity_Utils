using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	public abstract class VisualEffect : MonoBehaviour, IVisualEffect
	{
		public bool IsPlaying { get; private set; }

		public Transform Root => transform;



		public virtual void Dispose()
		{

		}
	}
}
