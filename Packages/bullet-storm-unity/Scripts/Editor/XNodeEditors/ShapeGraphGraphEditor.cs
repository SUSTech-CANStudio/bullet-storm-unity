using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using CANStudio.BulletStorm.XNodes;
using CANStudio.BulletStorm.XNodes.ShapeNodes;
using UnityEngine;
using XNode;
using XNodeEditor;
using Object = UnityEngine.Object;

namespace CANStudio.BulletStorm.Editor.XNodeEditors
{
    [CustomNodeGraphEditor(typeof(ShapeGraph), "BulletStorm_ShapeGraph.Settings")]
    public class ShapeAssetGraphEditor : NodeGraphEditor
    {
        private readonly Regex validNodes = new Regex(@"^CANStudio\.BulletStorm\.XNodes\.(Math|ShapeNodes)\..*$");
        private ShapeGraph shapeGraph;

        public override void OnCreate()
        {
            base.OnCreate();

            shapeGraph = target as ShapeGraph;
            if (shapeGraph is null || !shapeGraph) return;
            window.titleContent = new GUIContent($"{shapeGraph.name}", "The shape graph editor");
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
            if (type == typeof(Output) && shapeGraph.CheckOutputNode()) return null;
            return base.CreateNode(type, pos);
        }

        public override Node CopyNode(Node original)
        {
            return original is Output ? original : base.CopyNode(original);
        }

        public override void OnDropObjects(Object[] objects)
        {
            var pos = window.WindowToGridPosition(Event.current.mousePosition) - new Vector2(15, 15);
            var cnt = 0;
            var offset = new Vector2(10, 10);
            var add = false;
            Node node = null;

            foreach (var @object in objects)
            {
                switch (@object)
                {
                    case ShapeAsset asset:
                    {
                        node = CreateNode(typeof(ShapeReference), pos + offset * cnt++);
                        window.SelectNode(node, add);
                        add = true;
                        if (node is ShapeReference shapeReference)
                            shapeReference.shapeAsset = asset;
                        else BulletStormLogger.LogError("An unexpected errored occured when creating node.");
                        break;
                    }
                    default:
                        BulletStormLogger.Log($"{@object} can't drop into shape graph");
                        break;
                }

                Util.System.SendMessage(node, "OnValidate");
            }
        }

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
                    {typeof(Shape).PrettyName(), new Color(1, .7f, .8f)}
                }
            };
        }
    }
}