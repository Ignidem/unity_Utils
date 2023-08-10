using UnityEngine;

namespace Assets.External.unity_utils.Transforms
{
	public struct TransformInfo
	{
		public static implicit operator TransformInfo(Transform transform)
		{
			return new TransformInfo()
			{
				position = transform.position,
				rotation = transform.rotation.eulerAngles,
				scale = transform.lossyScale,
			};
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

		public Vector3Info position;
		public Vector3Info rotation;
		public Vector3Info scale;

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public void Apply(Transform transform)
		{
			ApplyPosition(transform);
			ApplyRotation(transform);
		}

		public void ApplyPosition(Transform transform)
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

		public void ApplyRotation(Transform transform)
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

		public void ApplyScale(Transform transform)
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
