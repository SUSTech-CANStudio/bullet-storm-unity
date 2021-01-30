using System;
using CANStudio.BulletStorm.Util;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem.Modules
{
    [Serializable]
    internal struct TracingModule
    {
        [Tooltip("Tracing target.")]
        [SerializeField] private Target target;

        [Tooltip("Max rotating angle per second.")]
        [MinValue(0), AllowNesting]
        [SerializeField] private float tracingRate;

        [SerializeField]
        private bool enableRateCurve;
        
        [Tooltip("x-axis is the angle between bullet's velocity, y-axis is the tracing rate multiplier")]
        [CurveRange(0, 0, 180, 1), SerializeField, ShowIf(nameof(enableRateCurve)), AllowNesting]
        private AnimationCurve tracingRateCurve;

        /// <summary>
        /// Call this on every update.
        /// </summary>
        /// <param name="bullet"></param>
        public void OnUpdate(IBulletController bullet)
        {
            if (!target.Check())
            {
                BulletStormLogger.LogWarningOnce($"Can not find {target}");
                return;
            }

            var deltaTime = Time.deltaTime;
            var targetPosition = target.AsTransform.position;
            var rate = tracingRate;
            var enableCurve = enableRateCurve;
            var curve = tracingRateCurve;
            bullet.ChangeParam(param =>
            {
                var aimRotation = Quaternion.LookRotation(targetPosition - param.position);
                var rateValue = enableCurve ? rate * curve.Evaluate(Quaternion.Angle(param.rotation, aimRotation)) : rate;
                if (rateValue < 0) rateValue = 0;
                param.rotation = Quaternion.RotateTowards(param.rotation,
                    aimRotation, rateValue * deltaTime);
                return param;
            });
        }
    }
}