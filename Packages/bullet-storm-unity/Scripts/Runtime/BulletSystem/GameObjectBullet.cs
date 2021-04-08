using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CANStudio.BulletStorm.Emission;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    ///     A bullet system based on <see cref="GameObject" />. It emits game objects as bullets,
    ///     and keeps them moving. The bullet system will attach a
    ///     <see cref="CANStudio.BulletStorm.BulletSystem.GameObjectBullet" />
    ///     component to every game objects it emits, from which you can get bullet attributes like
    ///     <see cref="CANStudio.BulletStorm.BulletSystem.BulletComponent.speed" /> and
    ///     <see cref="CANStudio.BulletStorm.BulletSystem.BulletComponent.lifetime" />.
    ///     <para />
    ///     This bullet system is much more inefficient than <see cref="ParticleBullet" />, but
    ///     useful when you need more flexibility. For example, if you need an enemy bullet which is
    ///     breakable by player's bullet, unfortunately unity's particle system can't detect collision
    ///     between two particles. A game object can be useful to deal with this situation.
    /// </summary>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class GameObjectBullet : BulletBase
    {
        [Tooltip("The game object to emit as bullet.")] [SerializeField]
        private GameObject bullet;

        [Tooltip("If disabled, bullets won't auto destroy.")] [SerializeField]
        private bool enableLifetime = true;

        [Tooltip("Default lifetime of bullets.")] [SerializeField]
        private float bulletLifeTime = 100;

        private List<BulletComponent> bullets = new List<BulletComponent>();
        private bool bulletsCleared;

        private void LateUpdate()
        {
            bulletsCleared = false;
        }

        public override void ChangeParam(Func<BulletParam, BulletParam> operation)
        {
            ClearDestroyedBullets();
            foreach (var gameObjectBullet in bullets)
            {
                var t = gameObjectBullet.transform;
                var param = operation(new BulletParam(t.rotation, t.position, gameObjectBullet.speed,
                    Time.time - gameObjectBullet.StartTime));
                t.SetPositionAndRotation(param.position, param.rotation);
                gameObjectBullet.speed = param.speed;
            }
        }

        public override void Emit(BulletEmitParam emitParam, Transform emitter)
        {
            var bulletComponent = Instantiate(bullet).AddComponent<BulletComponent>();
            bullets.Add(bulletComponent);
            var absEmitParam = emitParam.RelativeTo(emitter);
            bulletComponent.Init(absEmitParam.position, absEmitParam.velocity, absEmitParam.color, absEmitParam.size);
            if (enableLifetime) bulletComponent.EnableLifeTime(bulletLifeTime);

            PlayEmissionEffect(emitParam, emitter);
        }

        public override void Destroy()
        {
            if (this) StartCoroutine(WaitForDestroy());
        }

        private IEnumerator WaitForDestroy()
        {
            while (true)
            {
                if (bullets is null || bullets.Count == 0) break;
                yield return null;
            }

            Destroy(gameObject);
        }

        /// <summary>
        ///     Clear destroyed bullets if not cleared.
        /// </summary>
        private void ClearDestroyedBullets()
        {
            if (bulletsCleared) return;
            bullets = bullets.Where(gameObjectBullet => gameObjectBullet).ToList();
            bulletsCleared = true;
        }
    }
}