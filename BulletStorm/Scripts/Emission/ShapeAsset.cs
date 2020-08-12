using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BulletStorm.Emission
{
    [CreateAssetMenu(menuName = "BulletStorm/ShapeAsset")]
    public class ShapeAsset : ScriptableObject
    {
        public Shape shape;

        /// <summary>
        /// Creates an asset of given shape.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="assetPath"></param>
        public static void CreateAsset(Shape shape, string assetPath)
        {
#if UNITY_EDITOR
            var shapeAsset = CreateInstance<ShapeAsset>();
            shapeAsset.shape = shape;
            AssetDatabase.CreateAsset(shapeAsset, assetPath);
#else
            BulletStormLogger.LogError("Can't create asset in game.");
            return;
#endif
        }
    }
}