using NaughtyAttributes;
using UnityEngine;

namespace CANStudio.BulletStorm.Emission
{
    public class ShapeAsset : ScriptableObject, IShapeContainer
    {
        [ReorderableList]
        public Shape shape;

        public Shape GetShape() => shape;
    }
}