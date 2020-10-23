using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CANStudio.BulletStorm.BulletSystem;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.Emitters
{
    /// <summary>
    /// Provides simple config to emit bullets.
    /// </summary>
    [AddComponentMenu("BulletStorm/AutoBulletEmitter")]
    public class AutoBulletEmitter : AutoEmitterBase
    {
        [Header("Bullet config")]
        
        [Tooltip("Bullet system prefab to emit bullets."), SerializeField, Required]
        private BulletSystemBase bullet;
        
        [Tooltip("Total emit times."), MinValue(0)]
        public int emitTimes = 10;
        
        public ParticleSystem.MinMaxCurve emitInterval;
        public ParticleSystem.MinMaxCurve speed = 1;
        public ParticleSystem.MinMaxGradient color = Color.white;
        public ParticleSystem.MinMaxCurve size = 1;

        [Tooltip("When using curve or gradient, the time in seconds that curve x-axis 0~1 represents."),
         MinValue(1), ShowIf("ShowCurveTimeScale")]
        public float curveTimeScale = 1;

        [Header("Each emission")]

        [CurveRange(-1, -90, 1, 90),
         Tooltip("Pitch angle of every row, x-axis value '-1' is the first row (rows[0]), and '1' is the last row.")]
        public AnimationCurve rowAngles;
        
        [SerializeField, ReorderableList,
         Tooltip("Rows are sorted from top to down (y-axis), every row contains a row of bullets.")]
        private List<Row> rows;

        // for inspector use
        // ReSharper disable once UnusedMember.Local
        private bool ShowCurveTimeScale => emitInterval.mode == ParticleSystemCurveMode.Curve ||
                                           emitInterval.mode == ParticleSystemCurveMode.TwoCurves ||
                                           speed.mode == ParticleSystemCurveMode.Curve ||
                                           speed.mode == ParticleSystemCurveMode.TwoCurves ||
                                           color.mode == ParticleSystemGradientMode.Gradient ||
                                           color.mode == ParticleSystemGradientMode.TwoGradients ||
                                           size.mode == ParticleSystemCurveMode.Curve ||
                                           size.mode == ParticleSystemCurveMode.TwoCurves;

        protected override IEnumerator StartEmitCoroutine()
        {
            if (!CheckBullet()) yield break;
            var startTime = Time.time;
            for (var i = 0; i < emitTimes; i++)
            {
                var evaluateTime = (Time.time - startTime) / curveTimeScale;
                EmitOnce(speed.Evaluate(evaluateTime, Random.value), color.Evaluate(evaluateTime, Random.value),
                    size.Evaluate(evaluateTime, Random.value));
                yield return new WaitForSeconds(emitInterval.Evaluate(evaluateTime, Random.value));
            }
        }
        
        private bool CheckBullet()
        {
            if (bullet) return true;
            BulletStormLogger.LogError($"{this}: Bullet is empty!");
            return false;
        }
        
        /// <summary>
        /// Emits all configured bullets once.
        /// </summary>
        private void EmitOnce(float emitSpeed, Color emitColor, float emitSize)
        {
            // 0 <= seq < cnt
            static float SeqTo01(int seq, int cnt) => seq == 0 ? 0 : (float)seq / (cnt - 1);

            var paramList = new List<BulletEmitParam>[rows.Count];
            
            Parallel.For(0, rows.Count, i =>
            {
                paramList[i] = new List<BulletEmitParam>(rows[i].num);
                var pitch = rowAngles.Evaluate(SeqTo01(i, rows.Count) * 2 - 1);
                for (var j = 0; j < rows[i].num; j++)
                {
                    var yaw = rows[i].useCurve
                        ? rows[i].angles.Evaluate(SeqTo01(j, rows[i].num) * 2 - 1)
                        : rows[i].deltaAngle * (j - (rows[i].num - 1) / 2);
                    paramList[i].Add(new BulletEmitParam(
                        Vector3.zero,
                        Quaternion.Euler(pitch, yaw, 0) * Vector3.forward * emitSpeed,
                        emitColor, emitSize * Vector3.one));
                }
            });

            foreach (var param in paramList)
            {
                Emit(param, bullet);
            }
        }
        
        [Serializable]
        private struct Row
        {
            [Tooltip("Number of bullets in this row."), MinValue(0), AllowNesting]
            public int num;

            [Tooltip("Use curve to describe the distribution of bullets.")]
            public bool useCurve;
            
            [Tooltip("Angle between two bullets."), HideIf("useCurve"), MinValue(0), AllowNesting]
            public float deltaAngle;

            [CurveRange(-1, -180, 1, 180), ShowIf("useCurve"), AllowNesting,
             Tooltip("x-axis represents bullets, '-1' is the first one and '1' is the last one.\n" +
                     "y-axis is the angle of bullets, from left to right.")]
            public AnimationCurve angles;
        }
    }
}