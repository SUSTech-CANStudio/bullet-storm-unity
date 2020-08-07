using System.Collections;
using BulletStorm.BulletSystem;
using BulletStorm.Emission;
using BulletStorm.Util.EditorAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace BulletStorm.Emitters
{
    [AddComponentMenu("BulletStorm/AutoBulletEmitter")]
    public class AutoBulletEmitter : AutoEmitterBase
    {
        [Header("Bullet emitter")]
        [LocalizedTooltip("Bullet system prefab to emit bullets.")]
        [SerializeField] private BulletSystemBase bullet;
        [LocalizedTooltip("Total emit times.")]
        public int emitTimes = 10;
        [LocalizedTooltip("Interval between two emits.")]
        public ParticleSystem.MinMaxCurve emitInterval;
        [LocalizedTooltip("When using curve, the time in seconds that curve x-axis 0~1 represents.")]
        public float intervalCurveTimeScale = 1;

        [Header("Bullet parameter")]
        [LocalizedTooltip("Emit shapes instead of single bullets.")]
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
                    emitInterval.Evaluate(Time.time - startTime) / intervalCurveTimeScale);
            }
        }
    }
}