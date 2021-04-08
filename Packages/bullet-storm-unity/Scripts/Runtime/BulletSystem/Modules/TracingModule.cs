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
        [Tooltip("Tracing target.")] [SerializeField]
        private Target target;

        [Tooltip("Max rotating angle per second.")] [MinValue(0)] [AllowNesting] [SerializeField]
        private float tracingRate;

        [SerializeField] private bool enableRateCurve;

        [Tooltip("x-axis is the angle between bullet's velocity, y-axis is the tracing rate multiplier")]
        [CurveRange(0, 0, 180, 1)]
        [SerializeField]
        [ShowIf(nameof(enableRateCurve))]
        [AllowNesting]
        private AnimationCurve tracingRateCurve;

        /// <summary>
        ///     Call this on every update.
        /// </summary>
        /// <param name="bullet"></param>
        public void OnUpdate(IBulletController bullet)
        {
            if (!target.Check())
            {
                BulletStormLogger.LogWarningOnce($"Can not find {target}");
                return;
            }

            var targetPosition = target.AsTransform.position;
            var rate = tracingRate * Time.deltaTime;
            var enableCurve = enableRateCurve;
            var curve = tracingRateCurve;
            bullet.ChangeParam(param =>
            {
                var direction = param.rotation * Vector3.forward;
                var aimDirection = targetPosition - param.position;
                var axis = Vector3.Cross(direction, aimDirection);
                var dAngle = enableCurve ? curve.Evaluate(Vector3.Angle(direction, aimDirection)) * rate : rate;
                if (dAngle < 0)
                    dAngle = 0;
                param.rotation = Quaternion.AngleAxis(dAngle, axis) * param.rotation;
                return param;
            });
        }
    }
}