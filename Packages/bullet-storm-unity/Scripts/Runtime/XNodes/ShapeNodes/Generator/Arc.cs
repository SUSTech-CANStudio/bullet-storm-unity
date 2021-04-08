using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/Arc")]
    [NodeTint(Utils.ColorShapeGenerator)]
    public class Arc : ShapeNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public int num;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public float angle;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public float radius;

        public override void Generate()
        {
            SetShape(Shape.Arc(
                GetInputValue(nameof(num), num),
                GetInputValue(nameof(angle), angle),
                GetInputValue(nameof(radius), radius)));
        }
    }
}