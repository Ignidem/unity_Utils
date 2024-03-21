using UnityEngine;

namespace  UnityUtils.Prefabs
{
	public static class PrefabUtils
	{
		public static bool IsPrefab(this Object obj)
		{
			return obj is GameObject go ? !go.scene.IsValid() : false;
		}

		public static bool IsPrefab(this Component comp) => comp.gameObject.IsPrefab();

		public static Object GetPrefab(this Object obj) 
		{
#if UNITY_EDITOR
			return UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
#else
			throw new System.NotImplementedException();
#endif
		}
	}
}
