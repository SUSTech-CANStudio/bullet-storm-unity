using BulletStorm.Util.EditorAttributes;
using UnityEditor;
using UnityEngine;

namespace BulletStorm.Editor.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(CustomCurveAttribute))]
    public class CustomCurveDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (attribute is CustomCurveAttribute custom)
                EditorGUI.CurveField(position, property, custom.color, custom.range);
        }
    }
}