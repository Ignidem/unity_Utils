using UnityEngine;

namespace UnityUtils.Transforms
{
	[System.Serializable]
	public struct TransformInfo
	{
		public static TransformInfo LocalDefault = new TransformInfo() 
		{
			position = Vector3Info.LocalDefault,
			rotation = Vector3Info.LocalDefault,
			scale = new Vector3Info(Vector3.one, Space.Self) 
		};

		public static implicit operator TransformInfo(Transform transform)
		{
			return new TransformInfo(transform, Space.World);
		}
		public static implicit operator Matrix4x4(TransformInfo info)
		{
			return Matrix4x4.TRS(info.position, info.rotation, info.scale);
		}
		public static bool operator !=(TransformInfo left, TransformInfo right) => !(left == right);
		public static bool operator ==(TransformInfo left, TransformInfo right)
		{
			return left.position == right.position
				&& left.rotation == right.rotation
				&& left.scale == right.scale;
		}
		public static TransformInfo operator -(TransformInfo t1, TransformInfo t2)
		{
			return new TransformInfo()
			{
				position = t1.position - t2.position,
				rotation = t1.rotation - t2.rotation,
				scale = t1.scale - t2.scale,
			};
		}
		public static TransformInfo operator +(TransformInfo t1, TransformInfo t2)
		{
			return new TransformInfo()
			{
				position = t1.position + t2.position,
				rotation = t1.rotation + t2.rotation,
				scale = t1.scale + t2.scale,
			};
		}


		public readonly Vector3Info this[int i] => i switch
		{
			0 => position, 1 => rotation, 2 => scale,
			_ => throw new System.IndexOutOfRangeException()
		};

		public Vector3Info position;
		public Vector3Info rotation;
		public Vector3Info scale;

		public TransformInfo(Transform transform, Space space)
		{
			position = new Vector3Info(space == Space.World ? transform.position : transform.localPosition, space);
			rotation = new Vector3Info(space == Space.World ? transform.rotation.eulerAngles : transform.localRotation.eulerAngles, space);
			scale = new Vector3Info(space == Space.World ? transform.lossyScale : transform.localScale, space);
		}

		public override readonly bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override readonly int GetHashCode()
		{
			return base.GetHashCode();
		}

		public readonly void Apply(Transform transform)
		{
			ApplyPosition(transform);
			ApplyRotation(transform);
			ApplyScale(transform);
		}

		public readonly void ApplyPosition(Transform transform)
		{
			switch (position.space)
			{
				case Space.World:
					transform.position = position;
					break;
				case Space.Self:
					transform.localPosition = position;
					break;
			}
		}

		public readonly void ApplyRotation(Transform transform)
		{
			Quaternion rot = Quaternion.Euler(rotation);
			switch (rotation.space)
			{
				case Space.World:
					transform.rotation = rot;
					break;
				case Space.Self:
					transform.localRotation = rot;
					break;
			}
		}

		public readonly void ApplyScale(Transform transform)
		{
			switch (scale.space)
			{
				case Space.World:
					transform.localScale = scale;
					break;
				case Space.Self:
					transform.localScale = scale;
					break;
			}
		}
	}
}
