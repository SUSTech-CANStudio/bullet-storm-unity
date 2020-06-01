using ParticleStorm.Core;
using ParticleStorm.Factories;
using ParticleStorm.Util;
using System;
using UnityEngine;

namespace ParticleStorm
{
	/// <summary>
	/// <para>Particle is the bisic class of particle storm.</para>
	/// <para>A particle can be emitted, controlled, and add modules.</para>
	/// </summary>
	public class Particle : Named<Particle>, IParticle
	{
		public Particle() { InitParticleSystem(); }

		public Particle(string name) { Name = name; InitParticleSystem(); }
		
		public Particle(ParticlePrefeb prefeb)
		{
			InitParticleSystem();
			SetPrefeb(prefeb);
			Name = prefeb.name;
		}

		public Particle(string name, ParticlePrefeb prefeb)
		{
			InitParticleSystem();
			SetPrefeb(prefeb);
			Name = name;
		}

		~Particle()
		{
			if (particlePrefeb != null) { ParticlePrefeb.Destroy(particlePrefeb); }
		}

		/// <summary>
		/// Set a <see cref="ParticlePrefeb"/> to the particle.
		/// </summary>
		/// <param name="prefeb">The particle prefeb.</param>
		/// <param name="destroy">If the particle already have a prefeb, destroy the former prefeb when setting new prefeb.</param>
		/// <exception cref="ArgumentNullException"/>
		public void SetPrefeb(ParticlePrefeb prefeb, bool destroy = true)
		{
			if (prefeb is null || prefeb == default)
			{
				throw new ArgumentNullException(nameof(prefeb), "Particle prefeb can not be null.");
			}
			if (particlePrefeb != null && destroy)
			{
				ParticlePrefeb.Destroy(particlePrefeb);
			}
			particlePrefeb = prefeb;
			particlePrefeb.Bind(particleSystem);
		}

		/// <summary>
		/// Emit particles.
		/// </summary>
		/// <param name="emitParams">Initial parameters of the emitted particle.</param>
		/// <param name="num">Emit number.</param>
		public void Emit(EmitParams emitParams, int num) => particleSystem.Emit(emitParams, num);

		/// <summary>
		/// Initialize the particle.
		/// </summary>
		private void InitParticleSystem()
		{
			particleSystem = ParticleSystemFactory.PSParticleSystem();
		}
		
		/// <summary>
		/// Particle system object, can be realized with <see cref="ParticleSystem"/> or <see cref="GameObject"/>.
		/// </summary>
		private ParticleGenerator particleSystem;
		private ParticlePrefeb particlePrefeb;
	}
}
