using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Set Velocity", Utils.OrderVelocityOperation)]
    [NodeTint(Utils.ColorShapeOperation)]
    public class SetVelocity : ShapeOperationNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 velocity;

        public override void Generate()
        {
            CopyInputShape().SetVelocity(GetInputValue(nameof(velocity), velocity));
        }
    }
}