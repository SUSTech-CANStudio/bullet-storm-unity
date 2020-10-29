using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/ShapeReference")]
    public class ShapeReference : ShapeNode
    {
        [Required, AllowNesting]
        public ShapeAsset shapeAsset;
        
        public override void Generate()
        {
            SetShape(shapeAsset.shape);
        }
    }
}