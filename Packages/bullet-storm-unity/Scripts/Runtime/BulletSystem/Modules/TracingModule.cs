using System;
using CANStudio.BulletStorm.Util;
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
        [Range(0, 180)]
        [SerializeField] private float tracingRate;

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
            var ratio = this.tracingRate;
            bullet.ChangeVelocity((position, velocity) => 
                Vector3.RotateTowards(
                    velocity,
                    targetPosition - position,
                    ratio * deltaTime * Mathf.Deg2Rad,
                    0));
        }
    }
}