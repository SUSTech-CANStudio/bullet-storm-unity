using System;
using System.Collections.Generic;
using BulletStorm.BulletSystem;
using BulletStorm.Emission;
using BulletStorm.Storm;
using BulletStorm.Util;
using UnityEngine;

namespace BulletStorm.Emitters
{
    [AddComponentMenu("BulletStorm/Emitter")]
    public class Emitter : MonoBehaviour
    {
        private readonly Dictionary<IBulletSystem, IBulletController> bulletSystems =
            new Dictionary<IBulletSystem, IBulletController>();
        
        public void Emit(StormInfo storm)
        {
            
        }

        /// <summary>
        /// Emit bullets use given parameters from this emitter.
        /// </summary>
        /// <param name="shape">Bullet parameters</param>
        /// <param name="bullet">The bullet type to emit</param>
        public void Emit(IEnumerable<BulletEmitParam> shape, IBulletSystem bullet) =>
            Emit(shape, bullet, transform);

        /// <summary>
        /// Emit bullets use given parameters.
        /// </summary>
        /// <param name="shape">Bullet parameters</param>
        /// <param name="bullet">The bullet type to emit</param>
        /// <param name="emitter">Bullets will be emitted relative to it</param>
        public void Emit(IEnumerable<BulletEmitParam> shape, IBulletSystem bullet, Transform emitter)
        {
            foreach (var emitParam in shape) Emit(emitParam, bullet, emitter);
        }

        /// <summary>
        /// Emit a single bullet from this emitter.
        /// </summary>
        /// <param name="emitParam">Parameters of the bullet</param>
        /// <param name="bullet">The bullet type to emit</param>
        public void Emit(BulletEmitParam emitParam, IBulletSystem bullet) =>
            Emit(emitParam, bullet, transform);

        /// <summary>
        /// Emit a single bullet.
        /// </summary>
        /// <param name="emitParam">Parameters of the bullet</param>
        /// <param name="bullet">The bullet type to emit</param>
        /// <param name="emitter">Bullet will be emitted relative to it</param>
        public void Emit(BulletEmitParam emitParam, IBulletSystem bullet, Transform emitter) =>
            GetBulletSystem(bullet).Emit(emitParam, emitter);
        
        protected virtual void OnDestroy()
        {
            foreach (var copied in bulletSystems.Values) copied.Destroy();
        }

        private IBulletController GetBulletSystem(IBulletSystem bullet)
        {
            if (bulletSystems.TryGetValue(bullet, out var result)) return result;
            result = bullet.GetController();
            bulletSystems.Add(bullet, result);
            return result;
        }
    }
}