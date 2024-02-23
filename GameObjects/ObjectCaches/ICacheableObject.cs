using UnityEngine;

namespace UnityUtils.GameObjects.ObjectCaches
{
	public interface ICacheableObject
	{
		bool IsActive { get; }
		bool IsAlive { get; }
		bool IsStored => !IsActive;

		Transform Transform { get; }

		void Destroy();
		void OnPop(IObjectCache cache);
		void OnCached(IObjectCache cache);
		void Release();
	}
}
