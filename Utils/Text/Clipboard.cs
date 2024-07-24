using UnityEngine;

namespace UnityUtils.Text
{
	public static class Clipboard
	{
		public static void SetClipboard(this string value)
		{
			GUIUtility.systemCopyBuffer = value;
		}
		public static string GetClipboard()
		{
			return GUIUtility.systemCopyBuffer;
		}
	}
}
