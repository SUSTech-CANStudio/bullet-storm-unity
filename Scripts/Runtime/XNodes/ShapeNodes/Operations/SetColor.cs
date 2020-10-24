using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Set Color"), NodeTint(Utils.ColorShapeOperation)]
    public class SetColor : ShapeOperationNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Color color;
        
        public override void Generate()
        {
            SetShape(CopyInputShape().SetColor(GetInputValue(nameof(color), color)));
        }
    }
}