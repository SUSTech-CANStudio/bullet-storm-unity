using System;
using System.Collections;
using CANStudio.BulletStorm.BulletSystem;
using UnityEngine;

namespace CANStudio.BulletStorm.Storm.Actions
{
    /// <summary>
    /// Accelerates all bullets.
    /// </summary>
    [Serializable]
    public class Accelerate : IStormAction
    {
        [SerializeField] private bool customDirection;
        [SerializeField] private float acceleration;
        [SerializeField] private Vector3 accelerationVector;
        [SerializeField] private float duration;
        
        /// <summary>
        /// Accelerate in bullet original direction.
        /// </summary>
        /// <param name="acceleration">Speed change per second.</param>
        /// <param name="duration">Total time for the accelerate action.</param>
        public Accelerate(float acceleration, float duration)
        {
            customDirection = false;
            this.acceleration = acceleration;
            this.duration = duration;
        }

        /// <summary>
        /// Accelerate in customized direction.
        /// </summary>
        /// <param name="accelerationVector">Velocity change per second.</param>
        /// <param name="duration">Total time for the accelerate action.</param>
        public Accelerate(Vector3 accelerationVector, float duration)
        {
            customDirection = true;
            this.accelerationVector = accelerationVector;
            this.duration = duration;
        }
        
        public IEnumerator Execute(IBulletController controller, Transform emitter)
        {
            var startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                var deltaTime = Time.deltaTime;
                controller.ChangeVelocity((position, velocity) =>
                    velocity + (customDirection
                        ? accelerationVector * deltaTime
                        : velocity.normalized * (acceleration * deltaTime)));
                yield return null;
            }
        }
    }
}