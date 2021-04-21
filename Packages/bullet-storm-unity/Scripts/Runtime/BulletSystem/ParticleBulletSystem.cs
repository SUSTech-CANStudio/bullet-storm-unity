using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.Core;
using CANStudio.BulletStorm.Util;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    ///     A bullet system implemented using Unity <see cref="ParticleSystem" />.
    /// </summary>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    internal class ParticleBulletSystem : MonoBehaviour, IBulletSystemImplementation
    {
        private readonly RefList<BulletParams> _bullets = new RefList<BulletParams>();
        private readonly List<EmitParams> _emittingBullets = new List<EmitParams>();
        private bool _overrideColor, _overrideSize;

        private ParticleSystem.Particle[] _particles;

        private ParticleSystem _ps;
        private ParticleSystem.MainModule _psm;
        private ParticleSystemRenderer _psr;

        private void Start()
        {
            _ps = GetComponent<ParticleSystem>();
            _psm = _ps.main;
            _psr = GetComponent<ParticleSystemRenderer>();

            // disable particle system functions
            _psm.simulationSpace = ParticleSystemSimulationSpace.World;
            _ps.Pause(false);
            var emission = _ps.emission;
            emission.enabled = false;
            var shape = _ps.shape;
            shape.enabled = false;
        }

        private void LateUpdate()
        {
            WriteBullets();
            EmitBullets();
            _ps.Simulate(Time.deltaTime);
            ReadBullets();
        }

        public int BulletCount { get; private set; }

        public ref BulletParams Bullet(int index)
        {
            return ref _bullets[index];
        }

        public void Emit(EmitParams emitParams)
        {
            _emittingBullets.Add(emitParams);
        }

        public void Emit(IEnumerable<EmitParams> emitParams)
        {
            _emittingBullets.AddRange(emitParams);
        }

        public void Abandon()
        {
            StartCoroutine(WaitForDestroy());
        }

        /// <summary>
        ///     Creates a particle bullet system with parameters.
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="overrideColor">If true, use color in `Shape` instead of default color in particle system.</param>
        /// <param name="overrideSize">If true, use size in `Shape` instead of default size in particle system.</param>
        /// <returns></returns>
        public static ParticleBulletSystem Create(ParticleSystem prototype, bool overrideColor, bool overrideSize)
        {
            var go = Instantiate(prototype.gameObject);
            go.hideFlags = HideFlags.HideAndDontSave;
            var self = go.AddComponent<ParticleBulletSystem>();
            self._overrideColor = overrideColor;
            self._overrideSize = overrideSize;
            return self;
        }

        /// <summary>
        ///     Destroys this game object when no particle remains.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForDestroy()
        {
            while (true)
            {
                if (_ps.particleCount == 0) break;
                yield return null;
            }

            Destroy(gameObject);
        }

        /// <summary>
        ///     Read particles into bullets.
        /// </summary>
        private void ReadBullets()
        {
            // read particles
            if (_particles is null || _particles.Length < _psm.maxParticles)
                _particles = new ParticleSystem.Particle[_psm.maxParticles];
            BulletCount = _ps.GetParticles(_particles);

            // read bullets
            _bullets.Clear();
            for (var i = 0; i < BulletCount; i++)
                _bullets.Add(new BulletParams(_particles[i].position.ToSystem(),
                    Quaternion.LookRotation(_particles[i].velocity.Normalized()).ToSystem(),
                    _particles[i].velocity.magnitude,
                    _particles[i].startLifetime - _particles[i].remainingLifetime));
        }

        /// <summary>
        ///     Write bullets to particle system.
        /// </summary>
        private void WriteBullets()
        {
            if (BulletCount == 0) return;
            for (var i = 0; i < BulletCount; i++)
            {
                _particles[i].velocity =
                    (_bullets[i].rotation.ToUnity() * Vector3.forward).SafeChangeMagnitude(_bullets[i].speed);
                _particles[i].position = _bullets[i].position.ToUnity();
            }

            // also change rotation if particle is mesh
            if (_psr.renderMode == ParticleSystemRenderMode.Mesh)
                for (var i = 0; i < BulletCount; i++)
                    _particles[i].rotation3D = Quaternion.LookRotation(_particles[i].velocity).eulerAngles;

            _ps.SetParticles(_particles, BulletCount);
        }

        /// <summary>
        ///     Emit bullets in <see cref="_emittingBullets" />.
        /// </summary>
        private void EmitBullets()
        {
            foreach (var bullet in _emittingBullets)
            {
                var ep = new ParticleSystem.EmitParams
                {
                    position = bullet.position.ToUnity(),
                    velocity = bullet.velocity.ToUnity()
                };
                if (_overrideColor)
                    ep.startColor = new Color(bullet.color.X, bullet.color.Y, bullet.color.Z, bullet.color.W);
                if (_overrideSize) ep.startSize3D = bullet.size.ToUnity();
                _ps.Emit(ep, 1);
            }

            _emittingBullets.Clear();
        }
    }
}