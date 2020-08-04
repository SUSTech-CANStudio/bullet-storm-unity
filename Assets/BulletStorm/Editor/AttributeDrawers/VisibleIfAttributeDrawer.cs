using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using BulletStorm.Util.EditorAttributes;
using UnityEditor;
using UnityEngine;

namespace BulletStorm.Editor.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(VisibleIfAttribute))]
    internal class VisibleIfAttributeDrawer : PropertyDrawer
    {
        private bool hide;
        private bool warn;
        private object target;
        private MethodInfo method;
        private VisibleIfAttribute visible;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (hide) return 0;
            if (warn) return base.GetPropertyHeight(property, label) * 2;
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (visible is null)
            {
                visible = attribute as VisibleIfAttribute;
                if (visible is null) return;
            }

            if (target is null) target = GetParent(property);
            
            if (method is null) method = target.GetType().GetMethod(visible.funcName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            
            var result = method?.Invoke(target, null) as bool?;

            switch (result)
            {
                case null:
                    hide = false;
                    warn = true;
                    position.height /= 2;
                    EditorGUI.PropertyField(position, property, label);
                    position.y += position.height;
                    EditorGUI.HelpBox(position, "Invalid method '" + visible.funcName + "'", MessageType.Error);
                    break;
                case true:
                    hide = false;
                    warn = false;
                    EditorGUI.PropertyField(position, property, label);
                    break;
                case false:
                    hide = true;
                    warn = false;
                    break;
            }
        }
        
        private object GetParent(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach(var element in elements.Take(elements.Length-1))
            {
                if(element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }
        
        private object GetValue(object source, string name)
        {
            if(source == null)
                return null;
            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if(f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if(p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }
 
        private object GetValue(object source, string name, int index)
        {
            if (GetValue(source, name) is IEnumerable enumerable)
            {
                var enm = enumerable.GetEnumerator();
                while(index-- >= 0)
                    enm.MoveNext();
                return enm.Current;
            }

            return null;
        }
    }
}