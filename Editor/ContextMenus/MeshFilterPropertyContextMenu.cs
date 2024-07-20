using UnityEditor;
using UnityEngine;
using UnityUtils.Editor;
using UnityUtils.Editor.ContextMenus;

namespace UnityUtils.Effects.VisualEffects.Editor
{
	[PropertyContextMenu(typeof(Mesh))]
	public class MeshPropertyContextMenu : IPropertyContextMenu
	{
		public void AddPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			if (property.objectReferenceValue is not Mesh mesh || !mesh)
				return;

			menu.AddItem(new GUIContent("Save Mesh"), false, () => SaveMesh(mesh));
		}

		private void SaveMesh(Mesh mesh)
		{
			MeshUtility.Optimize(mesh);

			var obj = Selection.activeObject;
			string path = obj ? AssetDatabase.GetAssetPath(obj) : "Assets";
			path = EditorUtility.SaveFilePanel("Save " + mesh.name, path, mesh.name, "mesh");

			if (string.IsNullOrEmpty(path))
				return;

			AssetDatabase.CreateAsset(mesh, path.ToProjectPath());
		}
	}
}
