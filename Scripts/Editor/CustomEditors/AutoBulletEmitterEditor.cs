using CANStudio.BulletStorm.Emitters;
using UnityEditor;

namespace CANStudio.BulletStorm.Editor.CustomEditors
{
    [CustomEditor(typeof(AutoBulletEmitter))]
    internal class AutoBulletEmitterEditor : UnityEditor.Editor
    {
        private SerializedProperty useShapeProp;
        
        private void OnEnable()
        {
            useShapeProp = serializedObject.FindProperty("useShape");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            
            var it = serializedObject.GetIterator();
            it.NextVisible(true);
            while (it.NextVisible(false))
            {
                if (useShapeProp.boolValue && it.name == "emitParam") continue;
                if (!useShapeProp.boolValue && it.name == "shape") continue;
                
                EditorGUILayout.PropertyField(it);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}