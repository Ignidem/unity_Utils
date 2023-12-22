using UnityEditor;
using UnityEngine;

namespace Assets.External.unity_utils.Editor.ContextMenus
{
	internal class ScriptableObjectMenus
	{
		[MenuItem("Assets/Create/Scriptable Object", false, 0)]
		public static void CreateScriptableObject()
		{
			if (Selection.objects.Length == 1)
			{
				if (!TryGetScriptableScript(Selection.objects[0], out MonoScript script))
					return;

				System.Type type = script.GetClass();
				ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance(type), type.Name);
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
