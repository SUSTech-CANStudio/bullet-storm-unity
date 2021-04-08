using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/Rect")]
    [NodeTint(Utils.ColorShapeGenerator)]
    public class Rect : ShapeNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public float width;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public float height;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public int wNum;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public int hNum;

        public override void Generate()
        {
            SetShape(Shape.Rect(
                GetInputValue(nameof(width), width),
                GetInputValue(nameof(height), height),
                GetInputValue(nameof(wNum), wNum),
                GetInputValue(nameof(hNum), hNum)));
        }
    }
}