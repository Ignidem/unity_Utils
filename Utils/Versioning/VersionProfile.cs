using UnityEngine;
using UnityUtils.PropertyAttributes;
using Utils.Versioning;

namespace UnityUtils.Utils.Versioning
{
	public class VersionProfile : ScriptableObject
	{
		[SerializeField]
		private string versionString;
	}
}
