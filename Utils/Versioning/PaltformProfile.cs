using UnityEngine;

namespace UnityUtils.Utils.Versioning
{
	public class PaltformProfile : ScriptableObject
	{
		[SerializeField] private RuntimePlatform platform;
		[SerializeField] private CompileDefineCollection[] defines;
	}
}
