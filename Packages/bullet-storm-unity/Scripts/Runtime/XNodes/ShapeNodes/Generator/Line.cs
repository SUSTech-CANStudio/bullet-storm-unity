using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/Line"), NodeTint(Utils.ColorShapeGenerator)]
    public class Line : ShapeNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0), AllowNesting]
        public int num;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0), AllowNesting]
        public float length;
        
        public override void Generate()
        {
            SetShape(Shape.Line(
                GetInputValue(nameof(num), num),
                GetInputValue(nameof(length), length)));
        }
    }
}