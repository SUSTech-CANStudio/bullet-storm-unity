using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/Ring"), NodeTint(Utils.ColorShapeGenerator)]
    public class Ring : ShapeNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0), AllowNesting]
        public int num;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0), AllowNesting]
        public float radius;
        
        public override void Generate()
        {
            SetShape(Shape.Ring(
                GetInputValue(nameof(num), num),
                GetInputValue(nameof(radius), radius)));
        }
    }
}