using NaughtyAttributes;
using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Add Speed (by index)", Utils.OrderVelocityOperation), NodeTint(Utils.ColorShapeOperation)]
    public class AddSpeedByIndex : ShapeOperationNode
    {
        [Tooltip("X-axis 0~1 represents all indexes, y-axis is speed.")]
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited), CurveRange(0, 0, 1, 1), AllowNesting]
        public AnimationCurve curve;

        [Tooltip("Multiplier for curve y-axis.")]
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public float multiplier;
        
        public override void Generate()
        {
            SetShape(CopyInputShape()
                .AddSpeedByIndex(GetInputValue("curve", curve), GetInputValue(nameof(multiplier), multiplier)));
        }
    }
}