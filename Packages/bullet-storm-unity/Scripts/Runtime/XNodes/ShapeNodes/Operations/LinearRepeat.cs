using NaughtyAttributes;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Linear Repeat", Utils.OrderRepeatOperation)]
    [NodeTint(Utils.ColorShapeOperation)]
    public class LinearRepeat : ShapeOperationNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public int times;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public float length;

        public override void Generate()
        {
            SetShape(CopyInputShape().LinearRepeat(
                GetInputValue(nameof(times), times),
                GetInputValue(nameof(length), length)));
        }
    }
}