using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CANStudio.BulletStorm.BulletSystem;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.Emitters
{
    /// <summary>
    ///     Provides configurations to emit your shapes.
    /// </summary>
    [AddComponentMenu("BulletStorm/AutoShapeEmitter")]
    public class AutoShapeEmitter : AutoEmitterBase
    {
        [Header("Shape emitter")] [Tooltip("Bullet system prefab to emit bullets.")] [SerializeField] [Required]
        private BulletBase bullet;

        [SerializeField] [ProgressBar("Emission progress", "EmitTimes")]
        private int emitCount;

        [Tooltip("A list where you can config every emission here.")] [ReorderableList] [SerializeField]
        private ShapeConfig[] emissions;

        [ShowNativeProperty]
        private float TotalTime => emissions?.Aggregate(0f, (f, config) => f + config.TotalTime) ?? 0;

        #region reflection use only

        // ReSharper disable once UnusedMember.Local
        private float EmitTimes =>
            emissions?.Aggregate(0, (i, config) => i + config.RepeatTimes) ?? 0;

        #endregion

        protected override IEnumerator StartEmitCoroutine()
        {
            emitCount = 0;
            foreach (var emission in emissions)
            {
                var overriden = emission.OverridenShape;

                var repeatTimes = emission.repeat ? emission.repeatTimes : 1;

                if (emission.newReferenceSystem)
                    SetReferenceSystem(bullet, Emitter.rotation);

                for (var i = 0; i < repeatTimes; i++)
                {
                    if (overriden is null)
                        BulletStormLogger.LogWarning($"{this}: in emission element {emitCount}, shape not set");
                    else if (!emission.oneByOne) Emit(overriden, bullet);
                    else
                        for (var j = 0; j < overriden.Count; j++)
                        {
                            if (j != 0) yield return new WaitForSeconds(emission.interval);
                            Emit(overriden[j], bullet);
                        }

                    onEmit.Invoke();
                    emitCount++;
                    yield return new WaitForSeconds(emission.wait);
                }
            }
        }

        [Serializable]
        private struct ShapeConfig
        {
            [Required] [AllowNesting] public ShapeAsset shape;

            [Tooltip("Offset all bullets' start position.")]
            public Vector3 offset;

            [Tooltip("Override all bullets' speed of this shape.")]
            public bool overrideSpeed;

            [ShowIf("overrideSpeed")] [AllowNesting]
            public float speed;

            [Tooltip("Override all bullets' color of this shape.")]
            public bool overrideColor;

            [ShowIf("overrideColor")] [AllowNesting]
            public Color color;

            [Tooltip("Override all bullets' size of this shape.")]
            public bool overrideSize;

            [ShowIf("overrideSize")] [AllowNesting]
            public Vector3 size;

            [Tooltip("Emits bullets in this shape one by one.")]
            public bool oneByOne;

            [Tooltip("Time interval in second between two bullets' emission.")]
            [ShowIf("oneByOne")]
            [MinValue(0)]
            [AllowNesting]
            public float interval;

            [Tooltip("Emit this shape for many times.")]
            public bool repeat;

            [Tooltip("Repeat emitting this shape for how many times.")] [ShowIf("repeat")] [MinValue(1)] [AllowNesting]
            public int repeatTimes;

            [Tooltip("Wait time in second after finish each emission.")] [MinValue(0)] [AllowNesting]
            public float wait;

            [Tooltip("Record emitter's current rotation as reference system when this item begins.")]
            public bool newReferenceSystem;

            public IReadOnlyList<BulletEmitParam> OverridenShape
            {
                get
                {
                    if (!shape) return null;
                    var copy = shape.shape.Copy();
                    copy.Move(offset);
                    if (overrideSpeed) copy.SetSpeed(speed);
                    if (overrideColor) copy.SetColor(color);
                    if (overrideSize) copy.SetSize(size);
                    return copy;
                }
            }

            public int RepeatTimes => repeat ? repeatTimes : 1;

            public float TotalTime
            {
                get
                {
                    var time = 0f;
                    if (oneByOne && shape) time += shape.shape.Count > 0 ? (shape.shape.Count - 1) * interval : 0;
                    time += wait;
                    time *= RepeatTimes;
                    return time;
                }
            }
        }
    }
}