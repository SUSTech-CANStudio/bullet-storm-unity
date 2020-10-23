using System;
using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.BulletSystem;
using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.Emitters
{
    /// <summary>
    /// Provides configurations to emit your shapes.
    /// </summary>
    [AddComponentMenu("BulletStorm/AutoShapeEmitter")]
    public class AutoShapeEmitter : AutoEmitterBase
    {
        [Header("Shape emitter")]
        
        [Tooltip("Bullet system prefab to emit bullets."), SerializeField, Required]
        private BulletSystemBase bullet;

        [SerializeField, ProgressBar("Emission progress", "EmitTimes")]
        private int emitCount;
        
        [Tooltip("A list where you can config every emission here."), ReorderableList, SerializeField]
        private ShapeConfig[] emissions;

        #region reflection use only

        // ReSharper disable once UnusedMember.Local
        private float EmitTimes => emissions.Length;

        #endregion
        
        protected override IEnumerator StartEmitCoroutine()
        {
            emitCount = 0;
            foreach (var emission in emissions)
            {
                if (!emission.oneByOne) Emit(emission.OverridenShape, bullet);
                else
                {
                    foreach (var param in emission.OverridenShape)
                    {
                        Emit(param, bullet);
                        yield return new WaitForSeconds(emission.interval);
                    }
                }

                emitCount++;
                yield return new WaitForSeconds(emission.wait);
            }
        }
        
        [Serializable]
        private struct ShapeConfig
        {
            [Required, AllowNesting]
            public ShapeAsset shape;

            [Tooltip("Offset all bullets' start position.")]
            public Vector3 offset;
            
            [Tooltip("Override all bullets' speed of this shape.")]
            public bool overrideSpeed;

            [ShowIf("overrideSpeed"), AllowNesting]
            public float speed;
            
            [Tooltip("Override all bullets' color of this shape.")]
            public bool overrideColor;

            [ShowIf("overrideColor"), AllowNesting]
            public Color color;

            [Tooltip("Override all bullets' size of this shape.")]
            public bool overrideSize;

            [ShowIf("overrideSize"), AllowNesting]
            public Vector3 size;

            [Tooltip("Emits bullets in this shape one by one.")]
            public bool oneByOne;

            [Tooltip("Time interval in second between two bullets' emission."), ShowIf("oneByOne"),MinValue(0), AllowNesting]
            public float interval;

            [Tooltip("Wait time in second after finish this emission."), MinValue(0), AllowNesting]
            public float wait;

            public IEnumerable<BulletEmitParam> OverridenShape
            {
                get
                {
                    var copy = new Shape(shape.shape);
                    copy.Move(offset);
                    if (overrideSpeed) copy.SetSpeed(speed);
                    if (overrideColor) copy.SetColor(color);
                    if (overrideSize) copy.SetSize(size);
                    return copy;
                }
            }
        }
    }
}