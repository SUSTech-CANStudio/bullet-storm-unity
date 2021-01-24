using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    ///     All accessible parameters of a bullet in <see cref="IBulletController"/>.
    /// </summary>
    public struct BulletParam
    {
        /// <summary>
        ///     Rotation is also the speed direction of a bullet.
        /// </summary>
        public Quaternion rotation;
        
        /// <summary>
        ///     Position in world space.
        /// </summary>
        public Vector3 position;
        
        /// <summary>
        ///     Speed of the bullet, negative values represents a velocity towards negative direction.
        /// </summary>
        public float speed;
        
        /// <summary>
        ///     Scaled time from emission.
        /// </summary>
        public readonly float lifetime;

        internal BulletParam(Quaternion rotation, Vector3 position, float speed, float lifetime)
        {
            this.rotation = rotation;
            this.position = position;
            this.speed = speed;
            this.lifetime = lifetime;
        }
    }
}