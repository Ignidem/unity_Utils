using Unity.Cinemachine;
using UnityEngine;

namespace UnityUtils.Cameras.Cinemachine.Extensions
{
	public class CinemachineCursorVisibility : CinemachineExtension
	{
		[SerializeField]
		private bool isCursorVisible;

		public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
#if !UNITY_EDITOR
			Cursor.visible = isCursorVisible;
#endif
			return base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
		}

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{ }
	}
}
