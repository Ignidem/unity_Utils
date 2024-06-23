using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUtils.Cameras.Cinemachine.Extensions
{
	public class CinemachineTransitionEvent : CinemachineExtension
	{
		[SerializeField]
		private UnityEvent unityEvent;

		public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			unityEvent?.Invoke();
			return base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
		}

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
			CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{ }

	}
}
