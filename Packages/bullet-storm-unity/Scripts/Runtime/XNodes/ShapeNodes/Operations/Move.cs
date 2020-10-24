using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Move")]
    public class Move : ShapeOperationNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 offset;
        
        public override void Generate() => SetShape(CopyInputShape().Move(GetInputValue(nameof(offset), offset)));
    }
}