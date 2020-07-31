using System;
using UnityEngine;

#pragma warning disable 0649

namespace BulletStorm.BulletSystem.Modules
{
    [Serializable]
    internal struct TracingModule
    {
        [Tooltip("Enable bullets tracing some game object.")]
        [SerializeField] private bool enabled;
        [Tooltip("Tracing target.")]
        [SerializeField] private Transform target;
        [Tooltip("Max rotating angle per second.")]
        [Range(0, 180)]
        [SerializeField] private float tracingRatio;
        [Tooltip("Enable rotation of the bullet to change, or only change velocity direction.")]
        [SerializeField] private bool changeRotation;

        public void OnUpdate(IBulletSystem bulletSystem)
        {
            if (!enabled || target is null) return;

            var deltaTime = Time.deltaTime;
            var targetPosition = target.position;
            var ratio = this.tracingRatio;
            bulletSystem.ChangeVelocity((position, velocity) => 
                Vector3.RotateTowards(
                    velocity,
                    targetPosition - position,
                    ratio * deltaTime * Mathf.Deg2Rad,
                    0));
        }
    }
}