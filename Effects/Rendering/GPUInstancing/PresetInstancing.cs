using System;
using System.Linq;
using UnityEngine;
using UnityUtils.Transforms;

namespace UnityUtils.Effects.Rendering.GPUInstancing
{
	[Serializable]
	public class PresetInstancing : IInstancingMethod
	{
		[Serializable]
		public class Model
		{
			[SerializeField]
			private Mesh mesh;

			[SerializeField]
			private Material material;

			[SerializeField]
			private TransformInfo[] transforms;

			private Matrix4x4[] matrices;

			public void Setup()
			{
				matrices = transforms.Select(t => (Matrix4x4)t).ToArray();
			}

			public void Draw()
			{
				Graphics.DrawMeshInstanced(mesh, 0, material, matrices, matrices.Length);
			}
		}

		[SerializeField]
		private Model[] models;

		public void Validate() => Setup();

		public void Setup()
		{
			for (int i = 0; i < models.Length; i++)
			{
				Model model = models[i];
				model.Setup();
			}
		}

		public void Draw()
		{
			for (int i = 0; i < models.Length; i++)
			{
				Model model = models[i];
				model.Draw();
			}
		}
	}
}
