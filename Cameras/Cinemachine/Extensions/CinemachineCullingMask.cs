using Unity.Cinemachine;
using UnityEngine;

namespace UnityUtils.Cameras.Cinemachine.Extensions
{

	public class CinemachineCullingMask : CinemachineExtension
	{
		[SerializeField]
		private LayerMask cullMask;

		public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			UpdateCullingMask();
			return base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
		}

		private void UpdateCullingMask()
		{
			CinemachineBrain brain = CinemachineCore.FindPotentialTargetBrain(ComponentOwner);
			Camera camera = brain.OutputCamera;
			camera.cullingMask = cullMask;
		}

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, 
			CinemachineCore.Stage stage, ref CameraState state, float deltaTime) { }
	}
}
