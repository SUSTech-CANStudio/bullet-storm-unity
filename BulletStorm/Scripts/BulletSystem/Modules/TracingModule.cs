using System;
using BulletStorm.Util;
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
        [SerializeField] private Target target;
        [LocalizedTooltip("Max rotating angle per second.")]
        [Range(0, 180)]
        [SerializeField] private float tracingRate;

        public void OnStart() => target.Check();
        
        public void OnUpdate(IBulletController bullet)
        {
            if (!enabled || !target) return;

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