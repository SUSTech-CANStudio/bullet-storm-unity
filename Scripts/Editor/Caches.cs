using System;
using UnityEditor;
using UnityEngine;

namespace CANStudio.BulletStorm.Editor
{
    internal class Caches
    {
        public static Caches Instance => _instance.Value;
        
        public Mesh shapePreviewMesh;
        public Material shapePreviewMaterial;

        private static Lazy<Caches> _instance = new Lazy<Caches>(() => new Caches());
    }
}