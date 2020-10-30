using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Set Color (by index)", Utils.OrderColorOperation), NodeTint(Utils.ColorShapeOperation)]
    public class SetColorByIndex : ShapeOperationNode
    {
        [Tooltip("From left to right representing all indexes.")]
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Gradient gradient;
        
        public override void Generate()
        {
            SetShape(CopyInputShape().SetColorByIndex(GetInputValue(nameof(gradient), gradient)));
        }
    }
}