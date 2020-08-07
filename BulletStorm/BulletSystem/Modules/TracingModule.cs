using System;
using BulletStorm.Util.EditorAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace BulletStorm.BulletSystem.Modules
{
    [Serializable]
    internal struct TracingModule
    {
        [LocalizedTooltip("Enable bullets tracing some game object.")]
        [SerializeField] private bool enabled;
        [LocalizedTooltip("Tracing target.")]
        [SerializeField] private Transform target;
        [LocalizedTooltip("Max rotating angle per second.")]
        [Range(0, 180)]
        [SerializeField] private float tracingRatio;
        [LocalizedTooltip("Enable rotation of the bullet to change, or only change velocity direction.")]
        [SerializeField] private bool changeRotation;

        public void OnUpdate(IBulletController bullet)
        {
            if (!enabled || target is null) return;

            var deltaTime = Time.deltaTime;
            var targetPosition = target.position;
            var ratio = this.tracingRatio;
            bullet.ChangeVelocity((position, velocity) => 
                Vector3.RotateTowards(
                    velocity,
                    targetPosition - position,
                    ratio * deltaTime * Mathf.Deg2Rad,
                    0));
        }
    }
}