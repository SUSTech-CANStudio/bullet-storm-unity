using System;
using CANStudio.BulletStorm.XNodes.ShapeNodes;
using UnityEngine;
using XNodeEditor;

namespace CANStudio.BulletStorm.Editor.XNodeEditors
{
    [CustomNodeEditor(typeof(ShapeNode))]
    public class ShapeNodeNodeEditor : NodeEditor
    {
        private ShapeNode shapeNode;
        
        public override void OnCreate()
        {
            base.OnCreate();
            shapeNode = target as ShapeNode;
        }
        
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            serializedObject.Update();
            try
            {
                if (!shapeNode.IsShapeCurrent() && GUILayout.Button("Generate")) shapeNode.RecursiveGenerate();            }
            catch (ArgumentException)
            {
                // ignored
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}