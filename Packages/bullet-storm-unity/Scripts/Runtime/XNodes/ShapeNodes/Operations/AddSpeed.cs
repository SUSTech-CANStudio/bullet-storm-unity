using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Add Speed", Utils.OrderVelocityOperation)]
    [NodeTint(Utils.ColorShapeOperation)]
    public class AddSpeed : ShapeOperationNode
    {
        [Tooltip("Add speed to all bullets, direction is from origin to the bullet.")]
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public float speed;

        public override void Generate()
        {
            SetShape(CopyInputShape().AddSpeed(GetInputValue(nameof(speed), speed)));
        }
    }
}