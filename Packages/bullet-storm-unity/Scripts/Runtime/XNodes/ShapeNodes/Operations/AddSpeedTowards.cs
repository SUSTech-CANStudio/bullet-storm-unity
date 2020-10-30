using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Add Speed (towards point)", Utils.OrderVelocityOperation), NodeTint(Utils.ColorShapeOperation)]
    public class AddSpeedTowards : ShapeOperationNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public float speed;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 target;
        
        public override void Generate()
        {
            SetShape(CopyInputShape()
                .AddSpeedTowards(GetInputValue(nameof(speed), speed), GetInputValue(nameof(target), target)));
        }
    }
}