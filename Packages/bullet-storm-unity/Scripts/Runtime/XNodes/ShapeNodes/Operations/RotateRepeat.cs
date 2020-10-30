using NaughtyAttributes;
using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Rotate Repeat", Utils.OrderRepeatOperation), NodeTint(Utils.ColorShapeOperation)]
    public class RotateRepeat : ShapeOperationNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0), AllowNesting]
        public int times;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0), AllowNesting]
        public float angle;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 axis = Vector3.up;
        
        public override void Generate()
        {
            SetShape(CopyInputShape().RotateRepeat(
                GetInputValue(nameof(times), times),
                GetInputValue(nameof(angle), angle),
                GetInputValue(nameof(axis), axis)));
        }
    }
}