using System.Collections;
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
		[MenuItem("Assets/Utilities/List Missing Type References")]
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

	public static class ScriptableObjectMenus
	{
		[MenuItem("Assets/Create/Scriptable Object", false, 0)]
		public static void CreateScriptableObject()
		{
			if (Selection.objects.Length == 1)
			{
				if (!TryGetScriptableScript(Selection.objects[0], out MonoScript script))
					return;

				System.Type type = script.GetClass();
				ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance(type), type.Name + ".asset");
			}

			for (int i = 0; i < Selection.objects.Length; i++)
			{
				if (!TryGetScriptableScript(Selection.objects[i], out MonoScript script))
					return;

				System.Type type = script.GetClass();
				string directory = AssetDatabase.GetAssetPath(script);
				int index = directory.LastIndexOf('/') + 1;
				directory = directory[..index];
				string path = directory + type.Name + ".asset";
				AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(type), path);
			}
		}

		[MenuItem("Assets/Create/Scriptable Object", true)]
		public static bool CreateScriptableObjectValidate()
		{
			for (int i = 0; i < Selection.objects.Length; i++)
			{
				if (TryGetScriptableScript(Selection.objects[i], out _))
					return true;
			}

			return false;
		}
	
		private static bool TryGetScriptableScript(Object obj, out MonoScript script)
		{
			if (obj is MonoScript _script && typeof(ScriptableObject).IsAssignableFrom(_script.GetClass()))
			{
				script = _script;
				return true;
			}

			script = null;
			return false;
		}
	}
}
