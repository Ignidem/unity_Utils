using UnityEngine;

namespace Assets.External.unity_utils.Transforms
{
	public struct Vector3Info
	{
		public static implicit operator Vector3Info(Vector3 vect)
		{
			return new Vector3Info()
			{
				vector = vect,
				space = Space.World,
			};
		}

		public static implicit operator Vector3(Vector3Info info) => info.vector;

		public Vector3 vector;
		public Space space;
	}
}
