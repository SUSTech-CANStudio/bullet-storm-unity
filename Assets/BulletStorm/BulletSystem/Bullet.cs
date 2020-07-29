using JetBrains.Annotations;
using UnityEngine;

namespace BulletStorm.BulletSystem
{
    public class Bullet
    {
        private readonly IBulletSystem origin;
        
        public Bullet([NotNull] IBulletSystem bulletSystem)
        {
            origin = bulletSystem;
        }

        public IBulletSystem GetBulletSystem(Transform emitter)
        {
            return Object.Instantiate(origin as MonoBehaviour, emitter, false) as IBulletSystem;
        }

        public IBulletSystem GetOriginBulletSystem() => origin;
    }
}