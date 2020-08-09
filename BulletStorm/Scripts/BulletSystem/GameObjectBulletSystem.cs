using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BulletStorm.Emission;
using BulletStorm.Util.EditorAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace BulletStorm.BulletSystem
{
    /// <summary>
    /// A bullet system based on <see cref="GameObject"/>. It emits game objects as bullets,
    /// and keeps them moving. The bullet system will attach a <see cref="GameObjectBullet"/>
    /// component to every game objects it emits, from which you can get bullet attributes like
    /// <see cref="GameObjectBullet.velocity"/> and <see cref="GameObjectBullet.lifetime"/>.
    /// <para/>
    /// This bullet system is much more inefficient than <see cref="ParticleBulletSystem"/>, but
    /// useful when you need more flexibility. For example, if you need an enemy bullet which is
    /// breakable by player's bullet, unfortunately unity's particle system can't detect collision
    /// between two particles. A game object can be useful to deal with this situation. 
    /// </summary>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class GameObjectBulletSystem : BulletSystemBase
    {
        private List<GameObjectBullet> bullets = new List<GameObjectBullet>();
        private bool bulletsCleared;

        [LocalizedTooltip("The game object to emit as bullet.")]
        [SerializeField] private GameObject bullet;
        [LocalizedTooltip("If disabled, bullets won't auto destroy.")]
        [SerializeField] private bool enableLifetime = true;
        [LocalizedTooltip("Default lifetime of bullets.")]
        [SerializeField] private float bulletLifeTime = 100;

        public override void ChangePosition(Func<Vector3, Vector3, Vector3> operation)
        {
            ClearDestroyedBullets();
            foreach (var gameObjectBullet in bullets)
            {
                var bulletTransform = gameObjectBullet.transform;
                bulletTransform.position =
                    operation(bulletTransform.position, gameObjectBullet.velocity);
            }
        }

        public override void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation)
        {
            ClearDestroyedBullets();
            foreach (var gameObjectBullet in bullets)
            {
                gameObjectBullet.velocity = operation(gameObjectBullet.transform.position, gameObjectBullet.velocity);
            }
        }

        public override void Emit(BulletEmitParam emitParam, Transform emitter)
        {
            var bulletComponent = Instantiate(bullet).AddComponent<GameObjectBullet>();
            var absEmitParam = emitParam.RelativeTo(emitter);
            bulletComponent.Init(absEmitParam.position, absEmitParam.velocity, absEmitParam.color, absEmitParam.size);
            if (enableLifetime) bulletComponent.EnableLifeTime(bulletLifeTime);
            
            PlayEmissionEffect(emitParam, emitter);
        }

        public override void Destroy() => StartCoroutine(WaitForDestroy());

        private IEnumerator WaitForDestroy()
        {
            while (true)
            {
                if (bullets is null || bullets.Count == 0) break;
                yield return null;
            }
            Destroy(this);
        }

        /// <summary>
        /// Clear destroyed bullets if not cleared.
        /// </summary>
        private void ClearDestroyedBullets()
        {
            if (bulletsCleared) return;
            bullets = bullets.Where(gameObjectBullet => gameObjectBullet).ToList();
            bulletsCleared = true;
        }

        private void LateUpdate()
        {
            bulletsCleared = false;
        }

        private void OnDestroy()
        {
            ClearDestroyedBullets();
            foreach (var gameObjectBullet in bullets) Destroy(gameObjectBullet);
        }
    }
}