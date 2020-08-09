using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BulletStorm.Emission;
using UnityEngine;

namespace BulletStorm.BulletSystem
{
    [AddComponentMenu("")]
    public class GameObjectBulletSystem : BulletSystemBase
    {
        private List<GameObjectBullet> bullets = new List<GameObjectBullet>();
        private bool bulletsCleared;

        [Tooltip("The game object to emit.")]
        [SerializeField] private GameObject bullet;

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
            throw new NotImplementedException();
        }

        public override void Destroy()
        {
            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            while (true)
            {
                if (bullets is null || bullets.Count == 0) break;
                yield return null;
            }
            Destroy(this);
        }

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
    }
}