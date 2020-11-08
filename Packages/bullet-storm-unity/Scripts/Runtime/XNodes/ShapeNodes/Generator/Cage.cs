using CANStudio.BulletStorm.Emission;
using UnityEngine;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/Cage"), NodeTint(Utils.ColorShapeGenerator)]
    public class Cage : ShapeNode
    {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 size;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3Int count;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public bool edgeOnly;
        
        public override void Generate()
        {
            SetShape(Shape.Cage(
                GetInputValue(nameof(size), size),
                GetInputValue(nameof(count), count),
                GetInputValue(nameof(edgeOnly), edgeOnly)));
        }
    }
}