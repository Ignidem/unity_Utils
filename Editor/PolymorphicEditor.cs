using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using Utilities.Reflection;

namespace UnityUtils.Editor
{
	public class PolumorphicInfo
	{
		public readonly Type baseType;
		public readonly Type[] types;
		private readonly ConstructorInfo[] constructors;
		public string[] options;

		public int Index { get; set; }
		private int _index;

		public PolumorphicInfo(Type baseType)
		{
			this.baseType = baseType;
			types = baseType.GetSubTypes();
			options = types.Select(t => t.Name).ToArray();
			constructors = types.Select(t =>
				t.GetConstructor(new Type[] { baseType }) ?? t.GetConstructor(new Type[0])
			).ToArray();
		}

		public object Construct(int value, object currentObj)
		{
			ConstructorInfo cctor = constructors[value];

			if (cctor.GetParameters().Length == 0)
				return cctor.Invoke(new object[] { });

			return cctor.Invoke(new object[] { currentObj });
		}
	}

	public class PolymorphicEditor<T> : UnityEditor.Editor
	{
		private PolumorphicInfo info;

		protected virtual void OnEnable()
		{
			info = new PolumorphicInfo(typeof(T));
		}

		public override void OnInspectorGUI()
		{
			OnIndexChanged(EditorGUILayout.Popup(info.Index, info.options));

			base.OnInspectorGUI();
		}

		private void OnIndexChanged(int index)
		{
			if (info.Index == index) return;
		}
	}
}
