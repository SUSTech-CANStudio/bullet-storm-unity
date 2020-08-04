using BulletStorm.Emitters;
using UnityEditor;
using UnityEngine;

namespace BulletStorm.Editor
{
    [CustomPropertyDrawer(typeof(AutoEmitterBase.AutoAimModule))]
    public class AutoAimModuleDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (!property.isExpanded) return height;
            if (property.FindPropertyRelative("followRateUseCurve").boolValue)
            {
                height += BulletStormEditorUtil.ChildPropertiesHeight(property, "followRateConst");
            }
            else
            {
                height += BulletStormEditorUtil.ChildPropertiesHeight(property, "followRateCurve",
                    "followRateMultiplier");
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (!EditorGUI.PropertyField(new Rect(position) {height = EditorGUIUtility.singleLineHeight}, property
                , false)) return;
            position.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            EditorGUI.indentLevel++;
            
            var enabledProp = property.FindPropertyRelative("enabled");
            EditorGUI.PropertyField(BulletStormEditorUtil.GetPropertyPosition(ref position, enabledProp),
                enabledProp);
            var enabled = enabledProp.boolValue;
            
            EditorGUI.BeginDisabledGroup(!enabled);
            if (property.FindPropertyRelative("followRateUseCurve").boolValue)
            {
                BulletStormEditorUtil.DrawChildProperties(position, property, "followRateConst", "enabled");
            }
            else
            {
                BulletStormEditorUtil.DrawChildProperties(position, property, "followRateCurve", "followRateMultiplier", "enabled");
            }
            EditorGUI.EndDisabledGroup(); // !enabled
            
            EditorGUI.indentLevel--;
        }
    }
}