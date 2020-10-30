using CANStudio.BulletStorm.Emission;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Add", Utils.OrderSpecialOperation), NodeWidth(100), NodeTint(Utils.ColorShapeOperationSpecial)]
    public class Add : ShapeOperationNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
        public Shape addShape;

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