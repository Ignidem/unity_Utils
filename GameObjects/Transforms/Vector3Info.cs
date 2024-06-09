﻿using UnityEngine;

namespace UnityUtils.Transforms
{
	[System.Serializable]
	public struct Vector3Info
	{
		public static Vector3Info LocalDefault = new Vector3Info(Vector3.zero, Space.Self);

		public static implicit operator Vector3Info(Vector3 vect)
		{
			return new Vector3Info()
			{
				vector = vect,
				space = Space.World,
			};
		}
		public static bool operator !=(Vector3Info v1, Vector3Info v2) => !(v1 == v2);
		public static bool operator ==(Vector3Info v1, Vector3Info v2)
		{
			return v1.space == v2.space && v1.vector == v2.vector;
		}
		public static implicit operator Vector3(Vector3Info info) => info.vector;
		public static implicit operator Quaternion(Vector3Info info) => Quaternion.Euler(info.vector);
		public static Vector3Info operator +(Vector3Info v1, Vector3Info v2)
		{
			return new Vector3Info()
			{
				vector = v1.vector + v2.vector,
				space = v1.space,
			};
		}
		public static Vector3Info operator -(Vector3Info v1, Vector3Info v2)
		{
			return new Vector3Info()
			{
				vector = v1.vector - v2.vector,
				space = v1.space,
			};
		}

		public readonly float this[int i] => i switch
		{
			0 => vector.x,
			1 => vector.y,
			2 => vector.z,
			3 => (int)space,
			_ => throw new System.IndexOutOfRangeException()
		};

		public Vector3 vector;
		public Space space;

		public Vector3Info(Vector3 vector, Space space)
		{
			this.vector = vector;
			this.space = space;
		}

		public override readonly bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override readonly int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
