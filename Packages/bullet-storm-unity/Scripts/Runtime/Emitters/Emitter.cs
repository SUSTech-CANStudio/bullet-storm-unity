using System.Collections.Generic;
using CANStudio.BulletStorm.BulletSystem;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Storm;
using UnityEngine;

namespace CANStudio.BulletStorm.Emitters
{
    /// <summary>
    /// The basic emitter, all other emitters inherit this.
    /// This emitter doesn't have graphical interface, you have to call emit functions to emit bullets or storms.
    /// </summary>
    [AddComponentMenu("BulletStorm/Emitter")]
    public class Emitter : MonoBehaviour
    {
        private readonly Dictionary<IBulletSystem, IBulletController> bulletSystems =
            new Dictionary<IBulletSystem, IBulletController>();

        /// <summary>
        /// Emits a storm from this emitter.
        /// </summary>
        /// <param name="storm">The storm.</param>
        public void Emit(StormInfo storm) => Emit(storm, transform);
        
        /// <summary>
        /// Emits a storm.
        /// </summary>
        /// <param name="storm">The storm.</param>
        /// <param name="emitter">Bullets will be emitted relative to it.</param>
        public void Emit(StormInfo storm, Transform emitter) => StartCoroutine(storm.Execute(emitter));

        /// <summary>
        /// Emits bullets use given parameters from this emitter.
        /// </summary>
        /// <param name="shape">Bullet parameters</param>
        /// <param name="bullet">The bullet type to emit</param>
        public void Emit(IEnumerable<BulletEmitParam> shape, IBulletSystem bullet) =>
            Emit(shape, bullet, transform);

        /// <summary>
        /// Emits bullets use given parameters.
        /// </summary>
        /// <param name="shape">Bullet parameters</param>
        /// <param name="bullet">The bullet type to emit</param>
        /// <param name="emitter">Bullets will be emitted relative to it</param>
        public void Emit(IEnumerable<BulletEmitParam> shape, IBulletSystem bullet, Transform emitter)
        {
            foreach (var emitParam in shape) Emit(emitParam, bullet, emitter);
        }

        /// <summary>
        /// Emits a single bullet from this emitter.
        /// </summary>
        /// <param name="emitParam">Parameters of the bullet</param>
        /// <param name="bullet">The bullet type to emit</param>
        public void Emit(BulletEmitParam emitParam, IBulletSystem bullet) =>
            Emit(emitParam, bullet, transform);

        /// <summary>
        /// Emits a single bullet.
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