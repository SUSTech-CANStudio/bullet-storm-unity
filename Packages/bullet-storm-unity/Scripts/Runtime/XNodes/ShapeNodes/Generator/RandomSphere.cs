using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/Random Sphere"), NodeTint(Utils.ColorShapeGenerator)]
    public class RandomSphere : ShapeNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0), AllowNesting]
        public int num;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        [MinValue(0), AllowNesting]
        public float radius;

        public int seed;
        
        public override void Generate()
        {
            SetShape(Shape.RandomSphere(
                GetInputValue(nameof(num), num),
                GetInputValue(nameof(radius), radius),
                GetInputValue(nameof(seed), seed)));
        }
    }
}