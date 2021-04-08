using CANStudio.BulletStorm.Util;
using UnityEditor;
using UnityEngine;

namespace CANStudio.BulletStorm.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Target))]
    public class TargetDrawer : PropertyDrawer
    {
        private int last;

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            var left = new Rect(position) {width = position.width * 0.35f};
            var right = new Rect(position) {xMin = left.x + left.width};
            var findByProp = property.FindPropertyRelative("findBy");

            var save = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.PropertyField(left, findByProp, GUIContent.none);
            switch (findByProp.enumValueIndex)
            {
                case 0:
                    var transformProp = property.FindPropertyRelative("transform");
                    EditorGUI.PropertyField(right, transformProp, GUIContent.none);
                    break;
                case 1:
                    var infoProp = property.FindPropertyRelative("info");
                    if (last == 2) infoProp.stringValue = "";
                    infoProp.stringValue = EditorGUI.TextField(right, infoProp.stringValue);
                    last = 1;
                    break;
                case 2:
                    infoProp = property.FindPropertyRelative("info");
                    if (last == 1) infoProp.stringValue = "";
                    infoProp.stringValue = EditorGUI.TagField(right, infoProp.stringValue);
                    last = 2;
                    break;
            }

            EditorGUI.indentLevel = save;
            EditorGUI.EndProperty();
        }
    }
}