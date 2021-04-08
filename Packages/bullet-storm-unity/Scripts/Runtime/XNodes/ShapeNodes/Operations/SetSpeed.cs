using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Set Speed", Utils.OrderVelocityOperation)]
    [NodeTint(Utils.ColorShapeOperation)]
    public class SetSpeed : ShapeOperationNode
    {
        [Tooltip("Set speed to all bullets, direction is original direction.\n" +
                 "This requires bullets to have speed formally, if original speed is 0, this function won't change the speed.")]
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public float speed;

        public override void Generate()
        {
            SetShape(CopyInputShape().SetSpeed(GetInputValue(nameof(speed), speed)));
        }
    }
}