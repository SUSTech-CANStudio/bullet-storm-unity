using CANStudio.BulletStorm.Emission;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Add", -1000), NodeWidth(100)]
    public class Add : ShapeOperationNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
        public Shape addShape;

        public override bool IsShapeCurrent()
        {
            var adder = this.GetInputShapeNode(nameof(addShape));
            return !adder || base.IsShapeCurrent() && adder.IsShapeCurrent();
        }

        public override void RecursiveGenerate()
        {
            var adder = this.GetInputShapeNode(nameof(addShape));
            if (adder && !adder.IsShapeCurrent()) adder.RecursiveGenerate();
            base.RecursiveGenerate();
        }

        public override void Generate()
        {
            SetShape(CopyInputShape() + GetInputValue(nameof(addShape), addShape));
        }
    }
}