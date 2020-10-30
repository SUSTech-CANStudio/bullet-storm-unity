using CANStudio.BulletStorm.Emission;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    public abstract class ShapeOperationNode : ShapeNode
    {
        [SerializeField, Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
        private Shape inputShape;

        /// <summary>
        /// Convenient method to copy input shape.
        /// </summary>
        /// <returns></returns>
        protected Shape CopyInputShape() => GetInputValue(nameof(inputShape), inputShape).Copy();

        public override void RecursiveGenerate()
        {
            var last = this.GetInputShapeNode(nameof(inputShape));
            if (!IsShapeCurrent() && last) last.RecursiveGenerate();
            base.RecursiveGenerate();
        }
    }
}