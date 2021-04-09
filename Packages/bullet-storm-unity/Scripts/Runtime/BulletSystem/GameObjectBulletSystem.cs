using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BulletStorm.Core;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    ///     A _bulletPrototype system based on <see cref="GameObject" />. It emits game objects as _bulletComponents,
    ///     and keeps them moving. The _bulletPrototype system will attach a
    ///     <see cref="GameObjectBulletSystem" />
    ///     component to every game objects it emits, from which you can get _bulletPrototype attributes like
    ///     <see cref="BulletComponent.Speed" /> and
    ///     <see cref="BulletComponent.Lifetime" />.
    ///     <para />
    ///     This _bulletPrototype system is much more inefficient than <see cref="ParticleBulletSystem" />, but
    ///     useful when you need more flexibility. For example, if you need an enemy _bulletPrototype which is
    ///     breakable by player's _bulletPrototype, unfortunately unity's particle system can't detect collision
    ///     between two particles. A game object can be useful to deal with this situation.
    /// </summary>
    [AddComponentMenu("")]
    public class GameObjectBulletSystem : MonoBehaviour, IBulletSystemImplementation
    {
        private GameObject _bulletPrototype;
        private float _startLifetime;

        private List<BulletComponent> _bulletComponents = new List<BulletComponent>();

        private static readonly Lazy<GameObject> Daemon =
            new Lazy<GameObject>(() => new GameObject {hideFlags = HideFlags.HideAndDontSave});
        
        public int BulletCount => _bulletComponents.Count;
        
        /// <summary>
        ///     Create a game object bullet system with parameters.
        /// </summary>
        /// <param name="prototype">The game object to emit.</param>
        /// <param name="startLifetime">Lifetime of every bullets.</param>
        /// <returns></returns>
        public static GameObjectBulletSystem Create(GameObject prototype, float startLifetime)
        {
            var self = Daemon.Value.AddComponent<GameObjectBulletSystem>();
            self._bulletPrototype = prototype;
            self._startLifetime = startLifetime;
            return self;
        }

        public ref BulletParams Bullet(int index) => ref _bulletComponents[index].bulletParams;

        public void Emit(EmitParams emitParams) =>
            _bulletComponents.Add(BulletComponent.Create(_bulletPrototype, emitParams, _startLifetime));

        public void Emit(IEnumerable<EmitParams> emitParams)
        {
            foreach (var e in emitParams) Emit(e);
        }

        public void Abandon() => StartCoroutine(WaitForDestroy());

        private void LateUpdate()
        {
            _bulletComponents = _bulletComponents.Where(gameObjectBullet => gameObjectBullet).ToList();
        }

        private IEnumerator WaitForDestroy()
        {
            while (true)
            {
                if (_bulletComponents is null || _bulletComponents.Count == 0) break;
                yield return null;
            }

            Destroy(this);
        }
    }
}