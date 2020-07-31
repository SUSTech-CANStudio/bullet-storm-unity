using System.Collections.Generic;
using BulletStorm.BulletSystem;
using BulletStorm.Emission;
using BulletStorm.Storm;
using UnityEngine;

namespace BulletStorm.Emitters
{
    [AddComponentMenu("BulletStorm/Emitter")]
    public class Emitter : MonoBehaviour
    {
        private readonly Dictionary<IOriginBulletSystem, ICopiedBulletSystem> bulletSystems =
            new Dictionary<IOriginBulletSystem, ICopiedBulletSystem>();
        
        public void Emit(StormInfo storm)
        {
            
        }

        public void Emit(IEnumerable<BulletEmitParam> shape, IOriginBulletSystem bullet)
        {
            
        }

        /// <summary>
        /// Emit a single bullet.
        /// </summary>
        /// <param name="emitParam">Parameters of the bullet.</param>
        /// <param name="bullet">The bullet type to emit.</param>
        public void Emit(BulletEmitParam emitParam, IOriginBulletSystem bullet) =>
            GetBulletSystem(bullet).Emit(emitParam, transform);

        private ICopiedBulletSystem GetBulletSystem(IOriginBulletSystem bullet)
        {
            if (bulletSystems.TryGetValue(bullet, out var result)) return result;
            result = bullet.Copy();
            bulletSystems.Add(bullet, result);
            return result;
        }
    }
}