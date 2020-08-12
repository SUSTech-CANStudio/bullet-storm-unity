using System;
using System.Collections;
using BulletStorm.BulletSystem;
using UnityEngine;

namespace BulletStorm.Storm.Actions
{
    /// <summary>
    /// Rotates the velocity direction of bullets.
    /// </summary>
    [Serializable]
    public class Rotate : IStormAction
    {
        [SerializeField] private Vector3 angularVelocity;
        [SerializeField] private float duration;
        
        /// <summary>
        /// Rotates the velocity direction.
        /// </summary>
        /// <param name="angularVelocity">Rotates given angle per second.</param>
        /// <param name="duration">Total time of the rotation action.</param>
        public Rotate(Vector3 angularVelocity, float duration)
        {
            this.angularVelocity = angularVelocity;
            this.duration = duration;
        }
        
        public IEnumerator Execute(IBulletController controller, Transform emitter)
        {
            var startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                var deltaTime = Time.deltaTime;
                controller.ChangeVelocity((position, velocity) =>
                    Quaternion.Euler(angularVelocity * deltaTime) * velocity);
                yield return null;
            }
        }
    }
}