using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    public enum BulletSystemType
    {
        [Tooltip("Use Unity's ParticleSystem to run bullets.")]
        ParticleBulletSystem,

        [Tooltip("Use game object as bullet.")]
        GameObjectBulletSystem
    }
}