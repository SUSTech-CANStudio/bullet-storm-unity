using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Set Size", Utils.OrderSizeOperation)]
    [NodeTint(Utils.ColorShapeOperation)]
    public class SetSize : ShapeOperationNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 size;

        public override void Generate()
        {
            SetShape(CopyInputShape().SetSize(GetInputValue(nameof(size), size)));
        }
    }
}