#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityUtils.Editor;
using UnityUtils.RectUtils;

namespace UnityUtils.Effects.VisualEffects
{
    public abstract class ComponentPropertyDrawer<T> : ExtendedPropertyDrawer
    {
        protected T component;
        protected string[] names;
        protected int selected;

        protected virtual bool HasComponentChanged(T value)
        {
            if (component == null)
            {
                if (value == null)
                    return false;
				
                component = value;
                return true;
            }

            if (component.Equals(value)) 
                return false;
			
            component = value;
            return true;

        }
        protected abstract void UpdatePropertyList(string selectedName);

        protected virtual int GetValueId(string value)
        {
            return Shader.PropertyToID(value);
        }
		
        protected override float DrawProperty(ref Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty material = property.FindPropertyRelative("component");
            position = position.MoveY(LineHeight);
            EditorGUI.PropertyField(position, material);
            SerializedProperty propName = property.FindPropertyRelative("propertyName");
            SerializedProperty propHash = property.FindPropertyRelative("propertyHash");

            if (material.objectReferenceValue is not T comp)
                return 0;

            if (HasComponentChanged(comp))
                UpdatePropertyList(propName.stringValue);

            position = position.MoveY(LineHeight);
            int selected = EditorGUI.Popup(position, this.selected, names);
            if (selected != this.selected)
            {
                this.selected = selected;
                string name = names[this.selected];
                propName.stringValue = name;
                propHash.intValue = GetValueId(name);
                property.serializedObject.ApplyModifiedProperties();
            }

            return 0;
        }
    }
}
#endif