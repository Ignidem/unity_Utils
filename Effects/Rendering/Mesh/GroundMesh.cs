using UnityEngine;
using UnityUtils.GameObjects;
using Utilities.Numbers;

namespace UnityUtils.Effects.Rendering.MeshRendering
{
	public class GroundMesh : MonoBehaviour
	{
		[SerializeField] private MeshFilter filter;
		[SerializeField] private Texture2D noiseTexture;
		[SerializeField] private Vector2 heightRange;
		[SerializeField] private Vector2 size;
		[SerializeField] private Vector2Int count;

		private void Awake()
		{
			ReGenerateMesh();
		}

		private void OnValidate()
		{
			ReGenerateMesh();
		}

		private void ReGenerateMesh()
		{
			if (filter.sharedMesh)
			{
				filter.sharedMesh.DestroySelf();
			}

			filter.sharedMesh = GenerateMesh();
		}

		private Mesh GenerateMesh()
		{
			var _mesh = new Mesh();
			int lx = count.x + 1;
			int ly = count.y + 1;
			Vector3[] vertices = new Vector3[lx * ly];
			Vector2[] uv = new Vector2[lx * ly];
			int[] trigs = new int[lx * ly * 6];
			Vector2 spacing = new Vector2(size.x / lx, size.y / ly);

			void Next(ref int x, ref int y)
			{
				y++;
				if (y >= ly)
				{
					y = 0;
					x++;
				}
			}

			for (int x = 0, y = 0; y < ly && x < lx; Next(ref x, ref y))
			{
				int px = (lx * y) + x;
				float blx = x * spacing.x;
				float bly = y * spacing.y;
				Vector3 v = vertices[px] = GetPosition(blx - size.x/2, bly - size.y/2);

				float u = v.y.Remap(heightRange.x, heightRange.y, 0, 1);
				uv[px] = new Vector2(u, u);

				if (x == 0 || y == 0) continue;

				int d = px;
				int c = d - 1;
				int b = (lx * (y - 1)) + x;
				int a = b - 1;
				int ti = a * 6;

				trigs[ti] = c; trigs[ti + 1] = d; trigs[ti + 2] = a;
				trigs[ti + 3] = d; trigs[ti + 4] = b; trigs[ti + 5] = a;
			}

			_mesh.name = name;
			_mesh.vertices = vertices;
			_mesh.triangles = trigs;
			_mesh.uv = uv;
			_mesh.RecalculateNormals();
			return _mesh;
		}

		public Vector3 GetPosition(float x, float z)
		{
			return GetPosition(new Vector3(x, 0, z));
		}

		public Vector3 GetPosition(Vector3 point)
		{
			return new Vector3(point.x, GetHeight(point), point.z);
		}

		public float GetHeight(Vector3 localPosition)
		{
			Vector2 lc = new Vector2(
				localPosition.x * noiseTexture.width / size.x,
				localPosition.z  * noiseTexture.height / size.y
				);
			Vector2Int minlc = new Vector2Int((int)lc.x, (int)lc.y);
			//Vector2Int maxlc = new Vector2Int(Mathf.CeilToInt(lc.x), Mathf.CeilToInt(lc.y));
			
			Color minColor = noiseTexture.GetPixel(minlc.x, minlc.y);
			//Color maxColor = noiseTexture.GetPixel(maxlc.x, maxlc.y);
			Color color = minColor;
			//float t = (maxlc - minlc).magnitude;
			//Color color = Color.Lerp(minColor, maxColor, t);
			float d = Mathf.Max(color.r, color.g, color.b);
			return GetHeightLerp(d);
		}

		public float GetHeightLerp(float t)
		{
			return Mathf.Lerp(heightRange.x, heightRange.y, t);
		}
	}
}
