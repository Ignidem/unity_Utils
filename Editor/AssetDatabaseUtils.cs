using UnityEngine;

namespace UnityUtils.Editor
{
	public static class AssetDatabaseUtils
	{
		public static string ToProjectPath(this string devicePath)
		{
			string projectRootPath = Application.dataPath;
			int k = projectRootPath.LastIndexOf('/') + 1;

			string path = devicePath.StartsWith(projectRootPath) ? devicePath[k..] : devicePath;

			string dataFolder = projectRootPath[k..];
			if (!path.StartsWith(dataFolder))
				throw new System.Exception("Path is not in the project folder \n" + path);

			return path;
		}
	}
}
