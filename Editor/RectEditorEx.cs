using UnityEngine;

namespace UnityUtils.Editor
{
	public static class RectEditorEx
	{
		public static float RemainingWidth(Rect position) => ExtendedPropertyDrawer.ViewWidth - (position.x + 10);
	}
}
