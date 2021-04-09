using System.Collections;
using System.Collections.Generic;
using BulletStorm.Core;
using CANStudio.BulletStorm.Util;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
	/// <summary>
	///     A bullet system implemented using Unity <see cref="ParticleSystem" />.
	/// </summary>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class ParticleBulletSystem : MonoBehaviour, IBulletSystemImplementation
    {
        private bool _overrideColor, _overrideSize;
        
        private ParticleSystem _ps;
        private ParticleSystem.MainModule _psm;
        private ParticleSystemRenderer _psr;

        private ParticleSystem.Particle[] _particles;
        private BulletParams[] _bullets = new BulletParams[8];
        private readonly List<EmitParams> _emittingBullets = new List<EmitParams>();

        public int BulletCount { get; private set; }
        public ref BulletParams Bullet(int index) => ref _bullets[index];

        /// <summary>
        ///     Creates a particle bullet system with parameters.
        /// </summary>
        /// <param name="overrideColor">If true, use color in `Shape` instead of default color in particle system.</param>
        /// <param name="overrideSize">If true, use size in `Shape` instead of default size in particle system.</param>
        /// <returns></returns>
        public static ParticleBulletSystem Create(bool overrideColor, bool overrideSize)
        {
            var self = new GameObject(null, typeof(ParticleSystem)) {hideFlags = HideFlags.HideAndDontSave}
                .AddComponent<ParticleBulletSystem>();
            self._overrideColor = overrideColor;
            self._overrideSize = overrideSize;
            return self;
        }
        
        public void Emit(EmitParams emitParams) => _emittingBullets.Add(emitParams);
        public void Emit(IEnumerable<EmitParams> emitParams) => _emittingBullets.AddRange(emitParams);

        public void Abandon()
        {
            if (this) StartCoroutine(WaitForDestroy());
        }

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
            if (_bullets.Length < BulletCount)
            {
                // get the nearest power of 2 larger then `BulletCount`
                var size = (uint)BulletCount - 1;
                size |= size >> 1;
                size |= size >> 2;
                size |= size >> 4;
                size |= size >> 8;
                size |= size >> 16;
                _bullets = new BulletParams[size + 1];
            }
            for (var i = 0; i < BulletCount; i++)
            {
                _bullets[i] = new BulletParams(_particles[i].position.ToSystem(),
                    Quaternion.LookRotation(_particles[i].velocity.Normalized()).ToSystem(),
                    _particles[i].velocity.magnitude,
                    _particles[i].startLifetime - _particles[i].remainingLifetime);
            }
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
        ///     Emit bullets in <see cref="_emittingBullets"/>.
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