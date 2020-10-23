using UnityEngine;
using XNode;

namespace CANStudio.BulletStorm.Emission.XNodes
{
    public abstract class ShapeNode : Node, IShapeContainer
    {
        /// <summary>
        /// Every shape node contains a shape for output.
        /// </summary>
        [Output] public Shape shape;

        /// <summary>
        /// Generates the output shape.
        /// </summary>
        [ContextMenu("Generate", false, 0)]
        public abstract void Generate();

        public Shape GetShape() => shape;
    }
}