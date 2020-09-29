using System;
using System.Collections;
using CANStudio.BulletStorm.BulletSystem;
using UnityEngine;

namespace CANStudio.BulletStorm.Storm.Actions
{
    /// <summary>
    /// Rotate bullet velocity towards a transform or position.
    /// </summary>
    [Serializable]
    public class RotateTowards : IStormAction
    {
        [SerializeField] private bool useTransform;
        [SerializeField] private Transform transform;
        [SerializeField] private Vector3 position;
        [SerializeField] private float rate;
        [SerializeField] private float duration;
        
        /// <summary>
        /// Rotates velocity to look at a transform.
        /// </summary>
        /// <param name="transform">The target transform.</param>
        /// <param name="rate">Max rotate radians per second in degree.</param>
        /// <param name="duration">Total time of the rotation action.</param>
        public RotateTowards(Transform transform, float rate, float duration)
        {
            useTransform = true;
            this.transform = transform;
            this.rate = rate;
            this.duration = duration;
        }

        /// <summary>
        /// Rotates velocity to look at a position.
        /// </summary>
        /// <param name="position">The target position.</param>
        /// <param name="rate">Max rotate radians per second in degree.</param>
        /// <param name="duration">Total time of the rotation action.</param>
        public RotateTowards(Vector3 position, float rate, float duration)
        {
            useTransform = false;
            this.position = position;
            this.rate = rate;
            this.duration = duration;
        }
        
        public IEnumerator Execute(IBulletController controller, Transform emitter)
        {
            var startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                controller.ChangeVelocity((pos, vel) =>
                    Vector3.RotateTowards(vel, (useTransform ? transform.position : position) - pos, rate, 0));
                yield return null;
            }
        }
    }
}