using System.Collections;
using BulletStorm.BulletSystem;
using BulletStorm.Emission;
using UnityEngine;

#pragma warning disable 0649

namespace BulletStorm.Emitters
{
    [AddComponentMenu("BulletStorm/AutoBulletEmitter")]
    public class AutoBulletEmitter : AutoEmitterBase
    {
        [Header("Bullet emitter")]
        [Tooltip("Bullet system prefab to emit bullets.")]
        [SerializeField] private BulletSystemBase bullet;
        [Tooltip("Total emit times.")]
        [Range(1, 10000)]
        public int emitTimes = 10;
        [Tooltip("Interval between two emits.")]
        public ParticleSystem.MinMaxCurve emitInterval;
        [Tooltip("When using curve, the time in seconds that curve x-axis 0~1 represents.")]
        public float intervalCurveTimeScale = 1;
        [Tooltip("Multiplier for emit interval time.")]
        public float intervalMultiplier = 1;
        
        [Header("Bullet parameter")]
        [Tooltip("Emit shapes instead of single bullets.")]
        public bool useShape;
        public ShapeAsset shape;
        public BulletEmitParam emitParam;

        protected override IEnumerator StartEmitCoroutine()
        {
            var startTime = Time.time;
            for (var i = 0; i < emitTimes; i++)
            {
                if (useShape) Emit(shape.shape.AsReadOnly(), bullet, Emitter);
                else Emit(emitParam, bullet, Emitter);
                yield return new WaitForSeconds(
                    emitInterval.Evaluate((Time.time - startTime) / intervalCurveTimeScale) * intervalMultiplier);
            }
        }
    }
}