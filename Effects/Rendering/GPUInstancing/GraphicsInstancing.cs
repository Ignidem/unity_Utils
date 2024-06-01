using UnityEngine;
using UnityUtils.PropertyAttributes;

namespace UnityUtils.Effects.Rendering.GPUInstancing
{
	[ExecuteInEditMode]
	public class GraphicsInstancing : MonoBehaviour
	{
		[SerializeReference, Polymorphic]
		private IInstancingMethod method;

		public void OnValidate()
		{
			method.Validate();
		}

		public void OnEnable()
		{
			method.Setup();
		}

		public void Update()
		{
			method.Draw();
		}


	}
}
