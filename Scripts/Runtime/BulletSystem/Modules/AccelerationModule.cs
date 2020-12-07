using System;
using CANStudio.BulletStorm.Util;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem.Modules
{
    [Serializable]
    internal struct AccelerationModule
    {
        [Tooltip("Speed change per second."), SerializeField]
        private float acceleration;

        [Tooltip("Stop acceleration if speed lower than this."), SerializeField, ShowIf(nameof(ShowMinSpeed)), MinValue(0), AllowNesting]
        private float minSpeed;

        [Tooltip("Stop acceleration if speed higher than this."), SerializeField, ShowIf(nameof(ShowMaxSpeed)), MinValue(0), AllowNesting]
        private float maxSpeed;
        
        private bool ShowMinSpeed => acceleration < 0;
        private bool ShowMaxSpeed => acceleration > 0;

        public void OnUpdate(IBulletController controller)
        {
            var deltaTime = Time.deltaTime;
            var acc = acceleration;
            var min = minSpeed;
            var max = maxSpeed;
            controller.ChangeVelocity((oldPosition, oldVelocity) =>
            {
                var speed = oldVelocity.magnitude + acc * deltaTime;
                if (acc < 0 && speed < min) speed = min;
                else if (acc > 0 && speed > max) speed = max;
                return oldVelocity.SafeChangeMagnitude(speed);
            });
        }
    }
}