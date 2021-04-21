using System;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using CANStudio.BulletStorm.XNodes.ShapeNodes;
using UnityEngine;
using XNode;

namespace CANStudio.BulletStorm.XNodes
{
    [CreateAssetMenu(menuName = "BulletStorm/ShapeGraph")]
    public class ShapeGraph : NodeGraph, IShapeContainer
    {
        public Shape shape;

        private Output outputNode;

        public Shape GetShape()
        {
            return shape;
        }

        public bool CheckOutputNode()
        {
            if (!(outputNode is null) && outputNode && outputNode.graph == this) return true;
            return nodes.Exists(node =>
            {
                if (!(node is Output o)) return false;
                outputNode = o;
                return true;
            });
        }

        [ContextMenu("Build", false, 0)]
        public void Build()
        {
            if (CheckOutputNode()) outputNode.Build();
            else BulletStormLogger.LogError("Output node not found.", this);
        }

        public override Node AddNode(Type type)
        {
            if (type != typeof(Output) || !CheckOutputNode()) return base.AddNode(type);
            return outputNode;
        }
    }
}