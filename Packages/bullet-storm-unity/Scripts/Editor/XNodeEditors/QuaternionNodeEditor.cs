using CANStudio.BulletStorm.Util;
using CANStudio.BulletStorm.XNodes.Math.Noun;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace CANStudio.BulletStorm.Editor.XNodeEditors
{
    [CustomNodeEditor(typeof(Quaternion))]
    public class QuaternionNodeEditor : NodeEditor
    {
        private Quaternion quaternion;
        private SerializedProperty setUpward;
        private SerializedProperty type;
        private NodePort value;
        private SerializedProperty vector0;
        private SerializedProperty vector1;

        public override void OnCreate()
        {
            base.OnCreate();

            quaternion = target as Quaternion;
            if (quaternion is null || !quaternion)
            {
                BulletStormLogger.LogError("Failed to initiate editor.");
                return;
            }

            value = quaternion.GetOutputPort(nameof(value));
            type = serializedObject.FindProperty(nameof(type));
            setUpward = serializedObject.FindProperty(nameof(setUpward));
            vector0 = serializedObject.FindProperty(nameof(vector0));
            vector1 = serializedObject.FindProperty(nameof(vector1));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            NodeEditorGUILayout.PortField(value);
            NodeEditorGUILayout.PropertyField(type);
            switch (type.enumValueIndex)
            {
                case 0: // Euler
                    EditorGUILayout.PrefixLabel("Euler Angles");
                    NodeEditorGUILayout.PropertyField(vector0, quaternion.GetInputPort(nameof(vector0)));
                    break;
                case 1: // LookRotation
                    EditorGUILayout.PrefixLabel("Forward Direction");
                    NodeEditorGUILayout.PropertyField(vector0, quaternion.GetInputPort(nameof(vector0)));
                    NodeEditorGUILayout.PropertyField(setUpward);
                    if (setUpward.boolValue)
                    {
                        EditorGUILayout.PrefixLabel("Upward Direction");
                        NodeEditorGUILayout.PropertyField(vector1, quaternion.GetInputPort(nameof(vector1)));
                    }

                    break;
                case 2: // FromToRotation
                    EditorGUILayout.PrefixLabel("From Direction");
                    NodeEditorGUILayout.PropertyField(vector0, quaternion.GetInputPort(nameof(vector0)));
                    EditorGUILayout.PrefixLabel("To Direction");
                    NodeEditorGUILayout.PropertyField(vector1, quaternion.GetInputPort(nameof(vector1)));
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}