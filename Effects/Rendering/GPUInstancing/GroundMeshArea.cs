using UnityEngine;
using UnityUtils.Effects.Rendering.MeshRendering;
using UnityUtils.Transforms;

namespace UnityUtils.Effects.Rendering.GPUInstancing
{
	public class GroundMeshArea : IInstancingMethod
	{
		[SerializeField]
		private GroundMesh ground;

		[SerializeField]
		private Mesh mesh;

		[SerializeField]
		private Material material;

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
			matrices = new Matrix4x4[count];
			for (int i = 0; i < count; i++)
			{
				Vector3 pos = RandomBetween(min.position, max.position);
				if (ground)
					pos = ground.GetPosition(pos);

				matrices[i] = Matrix4x4.TRS(
					ground.transform.position + pos,
					ground.transform.rotation * Quaternion.Euler(RandomBetween(min.rotation, max.rotation)),
					RandomScale());
			}
		}

		private Vector3 RandomBetween(Vector3 min, Vector3 max)
		{
			return new Vector3(
				Random.Range(min[0], max[0]),
				Random.Range(min[1], max[1]),
				Random.Range(min[2], max[2])
				);
		}

		private Vector3 RandomScale()
		{
			int index = Random.Range(0, 3);
			float scale = Random.Range(min.scale[index], max.scale[index]);

			return ground.transform.localScale * scale;
		}

		public void Draw()
		{
			Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
		}
	}
}
