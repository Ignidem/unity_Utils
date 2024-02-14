using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtils.Asynchronous
{
	public static class TaskUtils
	{
		public static async void LogException(this Task task)
		{
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
