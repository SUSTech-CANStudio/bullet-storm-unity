using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    /// A bullet system based on <see cref="GameObject"/>. It emits game objects as bullets,
    /// and keeps them moving. The bullet system will attach a <see cref="GameObjectBullet"/>
    /// component to every game objects it emits, from which you can get bullet attributes like
    /// <see cref="GameObjectBullet.speed"/> and <see cref="GameObjectBullet.lifetime"/>.
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

        [Tooltip("The game object to emit as bullet.")]
        [SerializeField] private GameObject bullet;
        [Tooltip("If disabled, bullets won't auto destroy.")]
        [SerializeField] private bool enableLifetime = true;
        [Tooltip("Default lifetime of bullets.")]
        [SerializeField] private float bulletLifeTime = 100;

        public override void ChangePosition(Func<Vector3, Vector3, Vector3> operation)
        {
            ClearDestroyedBullets();
            foreach (var gameObjectBullet in bullets)
            {
                var t = gameObjectBullet.transform;
                t.position = operation(t.position, t.forward * gameObjectBullet.speed);
            }
        }

        public override void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation)
        {
            ClearDestroyedBullets();
            foreach (var gameObjectBullet in bullets)
            {
                var t = gameObjectBullet.transform;
                var velocity = operation(t.position, t.forward * gameObjectBullet.speed);
                t.forward = velocity.Normalized();
                gameObjectBullet.speed = velocity.magnitude;
            }
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
            var bulletComponent = Instantiate(bullet).AddComponent<GameObjectBullet>();
            bullets.Add(bulletComponent);
            var absEmitParam = emitParam.RelativeTo(emitter);
            bulletComponent.Init(absEmitParam.position, absEmitParam.velocity, absEmitParam.color, absEmitParam.size);
            if (enableLifetime) bulletComponent.EnableLifeTime(bulletLifeTime);
            
            PlayEmissionEffect(emitParam, emitter);
        }

        public override void Destroy()
        {
            if(this) StartCoroutine(WaitForDestroy());
        }

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