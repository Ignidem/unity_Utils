using System;
using UnityEngine;
using UnityUtils.Transforms;

namespace UnityUtils.Effects.Rendering.GPUInstancing
{
	public class AreaRandom : IInstancingMethod
	{
		[SerializeField]
		private Transform transform;

		[SerializeField]
		private Mesh mesh;

		[SerializeField]
		private Material material;

		[SerializeField]
		private LayerMask layer;

		[SerializeField]
		private int count;

		[SerializeField]
		private TransformInfo min;

		[SerializeField]
		private TransformInfo max;

		private Matrix4x4[] matrices;

		public void Validate()
		{
			Setup();
		}

		public void Setup()
		{
			System.Random rng = new System.Random();
			matrices = new Matrix4x4[count];
			for (int i = 0; i < count; i++)
			{
				matrices[i] = Matrix4x4.TRS(
					transform.position + RandomBetween(min.position, max.position),
					transform.rotation * Quaternion.Euler(RandomBetween(min.rotation, max.rotation)),
					RandomScale());
			}
		}

		private Vector3 RandomBetween(Vector3 min, Vector3 max)
		{
			return new Vector3(
				UnityEngine.Random.Range(min[0], max[0]),
				UnityEngine.Random.Range(min[1], max[1]),
				UnityEngine.Random.Range(min[2], max[2])
				);
		}

		private Vector3 RandomScale()
		{
			int index = UnityEngine.Random.Range(0, 3);
			float scale = UnityEngine.Random.Range(min.scale[index], max.scale[index]);

			return transform.localScale * scale;
		}

		public void Draw()
		{
			Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
		}
	}
}
