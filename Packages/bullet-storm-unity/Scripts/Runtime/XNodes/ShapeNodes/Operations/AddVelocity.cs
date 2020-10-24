using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Add Velocity")]
    public class AddVelocity : ShapeOperationNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 velocity;

        public override void Generate() => SetShape(CopyInputShape().AddVelocity(GetInputValue(nameof(velocity), velocity)));
    }
}