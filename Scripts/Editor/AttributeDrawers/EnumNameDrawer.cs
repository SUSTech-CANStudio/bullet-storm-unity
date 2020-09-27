using System;
using BulletStorm.Util.EditorAttributes;
using UnityEditor;
using UnityEngine;

namespace BulletStorm.Editor.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(EnumNameAttribute))]
    internal class EnumNameDrawer : PropertyDrawer
    {
        private string[] names;
        private bool isEnum;
        private bool initiated;
        
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (!initiated)
            {
                var type = fieldInfo.FieldType;
                if (type.IsEnum)
                {
                    names = property.enumDisplayNames;

                    var enumValues = type.GetEnumValues();

                    for (var i = 0; i < names.Length; i++)
                    {
                        var attr =
                            BulletStormEditorUtil.GetAttributeOfType<EnumNameAttribute>(enumValues.GetValue(i) as Enum);
                        if (!(attr.name is null) && attr.name.Length > 0)
                        {
                            names[i] = attr.name;
                        }
                    }

                    isEnum = true;
                }
                else
                {
                    isEnum = false;
                }
                initiated = true;
            }

            if (isEnum)
            {
                position = EditorGUI.PrefixLabel(position, label);
                property.enumValueIndex = EditorGUI.Popup(position, property.enumValueIndex, names);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}