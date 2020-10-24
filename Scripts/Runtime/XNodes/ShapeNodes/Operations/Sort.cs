using CANStudio.BulletStorm.Emission;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Sort", -1000)]
    public class Sort : ShapeOperationNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
        public ParamComparer comparer;

        public override void Generate()
        {
            SetShape(CopyInputShape().Sort(GetInputValue(nameof(comparer), comparer)));
        }
    }
}