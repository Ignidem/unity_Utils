using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.AddressableUtils;
using UnityUtils.GameObjects.ObjectCaches;

namespace UnityUtils.Effects.VisualEffects.Cache
{
	public class CachedVisualEffect : ICacheableObject
	{
		public bool IsActive => IsAlive && vfx.IsPlaying;

		public bool IsAlive => Transform;
		public Transform Transform => vfx.Root;
		
		public readonly IVisualEffect vfx;

		public CachedVisualEffect(IVisualEffect vfx)
		{
			this.vfx = vfx;
		}

		public void Destroy()
		{
			vfx.Dispose();
		}

		public void OnCached(IObjectCache cache)
		{
			throw new System.NotImplementedException();
		}

		public void OnPop(IObjectCache cache)
		{
			throw new System.NotImplementedException();
		}

		public void Release()
		{
			throw new System.NotImplementedException();
		}
	}
}
