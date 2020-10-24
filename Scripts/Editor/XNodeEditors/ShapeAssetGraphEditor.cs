using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.XNodes.ShapeNodes;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace CANStudio.BulletStorm.Editor.XNodeEditors
{
    [CustomNodeGraphEditor(typeof(ShapeAsset), "BulletStorm_ShapeAsset.Settings")]
    public class ShapeAssetGraphEditor : NodeGraphEditor
    {
        private readonly Regex validNodes = new Regex(@"^CANStudio\.BulletStorm\.XNodes\.(Math|ShapeNodes)\..*$");
        private ShapeAsset shapeAsset;

        public override void OnCreate()
        {
            base.OnCreate();
            
            shapeAsset = target as ShapeAsset;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            
            CreateNode(typeof(Output), Vector2.right * 300);
        }

        public override bool CanRemove(Node node)
        {
            return !(node is Output) && base.CanRemove(node);
        }

        public override Node CreateNode(Type type, Vector2 pos)
        {
            if (type == typeof(Output) && shapeAsset.CheckOutputNode()) return null;
            return base.CreateNode(type, pos);
        }

        public override Node CopyNode(Node original) => original is Output ? original : base.CopyNode(original);

        public override string GetNodeMenuName(Type type)
        {
            if (!validNodes.IsMatch(type.PrettyName()) || type == typeof(Output)) return null;
            
            return base.GetNodeMenuName(type);
        }

        public override NodeEditorPreferences.Settings GetDefaultPreferences()
        {
            return new NodeEditorPreferences.Settings
            {
                typeColors = new Dictionary<string, Color>
                {
                    {typeof(Shape).PrettyName(), new Color(1, .7f, .8f)},
                }
            };
        }
    }
}