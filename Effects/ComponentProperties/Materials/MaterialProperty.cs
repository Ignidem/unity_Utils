using System;
using UnityEngine;

namespace UnityUtils.Effects.VisualEffects
{
	[Serializable]
	public class MaterialProperty : ComponentProperty<Material>, ISerializationCallbackReceiver
	{
		[SerializeField] private Material material;

		public override void Set<T>(Material material, T value)
		{
			material.TrySetProperty(Hash, value, false);
		}

		public void OnBeforeSerialize()
		{
			if (material)
				component = material;
		}
		public void OnAfterDeserialize() { }
	}
}
