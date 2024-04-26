using UnityEditor;

namespace UnityUtils.Editor.ContextMenus
{
	public interface IPropertyContextMenu
	{
		void AddPropertyContextMenu(GenericMenu menu, SerializedProperty property);
	}
}
