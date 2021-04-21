using System;
using System.Collections.Generic;
using CANStudio.BulletStorm.BulletSystem;
using CANStudio.BulletStorm.Core;
using CANStudio.BulletStorm.Util;
using NaughtyAttributes;
using UnityEngine;

namespace CANStudio.BulletStorm
{
    [CreateAssetMenu(fileName = "BulletStorm/Bullet")]
    public class Bullet : ScriptableObject
    {
        [Header("Implementation")]
        public BulletSystemType implementation;
        
        [ShowIf(nameof(UseParticleBulletSystem)), Required]
        [Tooltip("Use particles in this particle system as bullets.")]
        public ParticleSystem particleSystem;

        [ShowIf(nameof(UseParticleBulletSystem))]
        [Tooltip("True to use size parameters in shape, false to use default size in particle system.")]
        public bool overrideSize;

        [ShowIf(nameof(UseParticleBulletSystem))]
        [Tooltip("True to use color parameters in shape, false to use default color in particle system.")]
        public bool overrideColor;
        
        [ShowIf(nameof(UseGameObjectBulletSystem)), Required]
        [Tooltip("Use this game object as every bullet.")]
        public GameObject gameObject;

        [ShowIf(nameof(UseGameObjectBulletSystem))]
        [Tooltip("Lifetime of bullets.")]
        public float bulletLifetime;
        
        [Header("Settings")]
        [SerializeReference]
        public IBulletAction[] modules;

        #region Reflection use only

        private bool UseGameObjectBulletSystem => implementation == BulletSystemType.GameObjectBulletSystem;
        private bool UseParticleBulletSystem => implementation == BulletSystemType.ParticleBulletSystem;

        #endregion

        internal IBulletSystemImplementation GetBulletSystem()
        {
            switch (implementation)
            {
                case BulletSystemType.ParticleBulletSystem:
                    if (!particleSystem) BulletStormLogger.LogError("Particle system not set.", this);
                    return ParticleBulletSystem.Create(particleSystem, overrideColor, overrideSize);
                case BulletSystemType.GameObjectBulletSystem:
                    if (!gameObject) BulletStormLogger.LogError("Game object not set.", this);
                    return GameObjectBulletSystem.Create(gameObject, bulletLifetime);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal List<IBulletAction> GetModules()
        {
            throw new NotImplementedException();
        }
    }
}