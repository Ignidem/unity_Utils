using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.Asynchronous
{
	public static class TaskUtils
	{
		public static async void LogException(this Task task)
		{
			if (task == null) return;

			try
			{
				await task;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}
	}
}
