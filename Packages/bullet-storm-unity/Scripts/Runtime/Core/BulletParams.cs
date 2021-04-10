using System.Numerics;

namespace CANStudio.BulletStorm.Core
{
    public struct BulletParams
    {
        /// <summary>
        ///     Position in world space.
        /// </summary>
        public Vector3 position;

        /// <summary>
        ///     Rotation is also the speed direction of a bullet.
        /// </summary>
        public Quaternion rotation;

        /// <summary>
        ///     Speed of the bullet, negative values represents a velocity towards negative direction.
        /// </summary>
        public float speed;

        /// <summary>
        ///     Scaled time from emission.
        /// </summary>
        public readonly float lifetime;

        public BulletParams(Vector3 position, Quaternion rotation, float speed, float lifetime)
        {
            this.position = position;
            this.rotation = rotation;
            this.speed = speed;
            this.lifetime = lifetime;
        }
    }
}