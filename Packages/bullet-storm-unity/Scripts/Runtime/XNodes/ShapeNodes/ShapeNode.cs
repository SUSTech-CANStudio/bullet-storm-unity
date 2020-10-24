using CANStudio.BulletStorm.Emission;
using UnityEngine;
using XNode;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    public abstract class ShapeNode : Node, IShapeContainer
    {
        /// <summary>
        /// Every shape node contains a shape for output.
        /// </summary>
        [SerializeField, Output(typeConstraint = TypeConstraint.Inherited)]
        private Shape shape;
        
        [SerializeField, HideInInspector]
        private bool dirty;
        
        /// <summary>
        /// Generates the output shape.
        /// This function only generates the selected node, even if previous node not generated.
        /// </summary>
        public abstract void Generate();

        public Shape GetShape() => shape;
        public void SetShape(Shape value) => shape = value;

        /// <summary>
        /// True if shape in this node is generated and latest.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsShapeCurrent() => !dirty;

        /// <summary>
        /// Deal with output <see cref="shape"/>.
        /// If there are other values, override it.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                case "shape":
                    return shape;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Provides a recursive way to generate and refresh shapes in previous nodes.
        /// </summary>
        public virtual void RecursiveGenerate()
        {
            Generate();
            dirty = false;
        }

        [ContextMenu("Generate", false, 0)]
        internal void GenerateButton() => RecursiveGenerate();

        private void OnValidate() => dirty = true;

        public override void OnCreateConnection(NodePort @from, NodePort to)
        {
            dirty = true;
            base.OnCreateConnection(@from, to);
        }

        public override void OnRemoveConnection(NodePort port)
        {
            dirty = true;
            base.OnRemoveConnection(port);
        }
    }
}