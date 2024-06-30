using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace WarmongersAPI.External.unity_utils.Editor.ContextMenus
{
	public static class MissingReferenceTypesMenus
	{
		//[MenuItem("Assets/Utilities/List Missing Type References")]
		public static void ShowMissingReferenceTypesMenus()
		{
			string enginePath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
			string mainDir = Directory.GetCurrentDirectory() + '/';
			foreach (var folderPath in GetSubFolders(enginePath))
			{
				var dir = new DirectoryInfo(mainDir + folderPath);
				foreach (var file in dir.GetFiles())
				{
					if (file.Extension is not ".prefab" and not ".asset")
						continue;
					
					string assetPath = enginePath + '/' + file.Name;
					Object[] objs = AssetDatabase.LoadAllAssetsAtPath(assetPath);
					foreach (var obj in objs)
					{
						Debug.Log(obj);
						bool hasMissingTypes = obj switch
						{
							GameObject gameObject => HasAnyMissingTypes(gameObject.transform),
							null => true,
							_ => SerializationUtility.GetManagedReferencesWithMissingTypes(obj).Length > 0,
						};

						if (hasMissingTypes)
						{
							Debug.LogWarning($"<a href=\"{assetPath}\" line=\"2\">{file.Name}</a>");
							break;
						}
					}
				}
			}
		}

		private static IEnumerable<string> GetSubFolders(string path)
		{
			string[] paths = AssetDatabase.GetSubFolders(path);
			yield return path;

			for (int i = 0; i < paths.Length; i++)
			{
				foreach (var sub in GetSubFolders(paths[i]))
					yield return sub;
			}
		}

		private static bool HasAnyMissingTypes(Transform transform)
		{
			foreach (var comp in transform.GetComponents(typeof(Component)))
			{
				if (comp is not MonoBehaviour and not UIBehaviour)
					continue;

				long[] ids = ManagedReferenceUtility.GetManagedReferenceIds(comp);
				foreach (var id in ids)
				{
					var obj = ManagedReferenceUtility.GetManagedReference(comp, id);
					if (obj == null)
						return true;
				}
			}

			int childcount = transform.childCount;
			for (int i = 0; i < childcount; i++)
			{
				var child = transform.GetChild(i);
				if (HasAnyMissingTypes(child))
					return true;
			}

			return false;
		}
	}
}
