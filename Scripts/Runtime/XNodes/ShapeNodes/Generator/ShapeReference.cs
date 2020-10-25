using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
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

        protected override void OnValidate()
        {
            if (shapeAsset.Equals(graph))
            {
                shapeAsset = null;
                BulletStormLogger.LogWarning("Shape asset can not reference itself.");
            }
            base.OnValidate();
        }
    }
}