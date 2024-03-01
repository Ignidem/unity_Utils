using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.AddressableUtils;
using UnityUtils.GameObjects.ObjectCaches;

namespace UnityUtils.Effects.VisualEffects.Cache
{
	public class VisualEffectCache : BaseObjectCache<IAddressableKey, CachedVisualEffect>,
		IAsyncObjectCache<IAddressableKey, CachedVisualEffect>
	{
		public VisualEffectCache(Transform parent = null, bool withController = false)
			: base(parent, withController) { }

		public Task<CachedVisualEffect> this[IAddressableKey key] => this.PopOrCreate(key);

		public async Task<CachedVisualEffect> Create(IAddressableKey key)
		{
			IAddressable<IVisualEffect> vfxAdrs = await key.Load<IVisualEffect>();
			return new CachedVisualEffect(vfxAdrs.Target);
		}
	}
}
