using UnityEngine;
using Utilities.Collections;

namespace UnityUtils.GameObjects
{
	public class PlatformAvailability : MonoBehaviour
	{
		public bool IsAvailable => platforms.Contains(Application.platform);

		[SerializeField]
		private RuntimePlatform[] platforms;

		private void OnEnable()
		{
			gameObject.SetActive(IsAvailable);
		}
	}
}
