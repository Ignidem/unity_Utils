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

		public Vector3Info position;
		public Vector3Info rotation;
		public Vector3Info scale;

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
