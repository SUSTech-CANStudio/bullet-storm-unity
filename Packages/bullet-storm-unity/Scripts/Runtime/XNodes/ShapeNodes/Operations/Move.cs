using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Move", Utils.OrderPositionOperation), NodeTint(Utils.ColorShapeOperation)]
    public class Move : ShapeOperationNode
    {
        [Tooltip("Move all bullets by offset.")]
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 offset;

        [Tooltip("Move every bullets with its velocity by time.")]
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public float time;
        
        public override void Generate() => SetShape(CopyInputShape()
            .Move(GetInputValue(nameof(offset), offset))
            .Move(GetInputValue(nameof(time), time)));
    }
}