using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityUtils.AddressableUtils
{
	public static class AddressableUtilities
	{
		public static TaskAwaiter<T> GetAwaiter<T>(this AsyncOperationHandle<T> operation)
		{
			return operation.Task.GetAwaiter();
		}

		private static readonly System.Type unityObject = typeof(Object);
		private static readonly System.Type unityComponent = typeof(Component);
		public static bool IsComponentType(this System.Type type)
		{
			return unityComponent.IsAssignableFrom(type) || !unityObject.IsAssignableFrom(type);
		}
	}
}
