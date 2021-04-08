using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/Fibonacci Sphere")]
    [NodeTint(Utils.ColorShapeGenerator)]
    public class FibonacciSphere : ShapeNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public int num;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0)]
        [AllowNesting]
        public float radius;

        public override void Generate()
        {
            SetShape(Shape.FibonacciSphere(GetInputValue(nameof(num), num), GetInputValue(nameof(radius), radius)));
        }
    }
}