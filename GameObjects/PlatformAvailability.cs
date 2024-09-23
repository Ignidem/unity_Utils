using UnityEngine;
using Utilities.Collections;

namespace UnityUtils.GameObjects
{
	public class PlatformAvailability : MonoBehaviour
	{
		[SerializeField]
		private RuntimePlatform[] platforms;

		private void OnEnable()
		{
			gameObject.SetActive(platforms.Contains(Application.platform));
		}
	}
}
