using System.Runtime.CompilerServices;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityUtils.AddressableUtils
{
	public static class AsyncOperatinHandleEx
	{
		public static TaskAwaiter<T> GetAwaiter<T>(this AsyncOperationHandle<T> operation)
		{
			return operation.Task.GetAwaiter();
		}
	}

	/*
	[CustomPropertyDrawer(typeof(AsyncAsset))]
	public class AsyncAssetPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty asset = property.FindPropertyRelative("reference");
			EditorGUI.PropertyField(position, asset, label);
		}
	}
	*/
}
