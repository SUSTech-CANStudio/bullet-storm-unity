using BulletStorm.Emitters;
using UnityEditor;
using UnityEngine;

namespace BulletStorm.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(AutoEmitterBase.AimOffsetModule))]
    internal class AimOffsetModuleDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (property.isExpanded)
            {
                height += BulletStormEditorUtil.ChildPropertiesHeight(property);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {

            if (!EditorGUI.PropertyField(new Rect(position) {height = EditorGUIUtility.singleLineHeight}, property,
                false)) return;

            position.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            EditorGUI.indentLevel++;
            var enabledProp = property.FindPropertyRelative("enabled");
            EditorGUI.PropertyField(BulletStormEditorUtil.GetPropertyPosition(ref position, enabledProp), enabledProp);
            EditorGUI.BeginDisabledGroup(!enabledProp.boolValue);
            BulletStormEditorUtil.DrawChildProperties(position, property, "enabled");
            EditorGUI.EndDisabledGroup(); // !enabledProp.boolValue
            EditorGUI.indentLevel--;
        }
    }
}