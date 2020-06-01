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
		/// Set trigger collider for the particle.<para/>
		/// Only when you enabled `useParticleSystem` in <see cref="particlePrefeb"/>,
		/// you need to call this function to add trigger colliders for particles.<para/>
		/// For particle based on <see cref="GameObject"/>, all colliders in the sence are
		/// able to collide.
		/// </summary>
		/// <param name="index">Collider index.</param>
		/// <param name="collider"></param>
		public void SetTriggerCollider(int index, Collider collider)
		{
			((PSParticleSystem)particleSystem).SetTriggerCollider(index, collider);
		}

		/// <summary>
		/// Get trigger collider of the particle.<para/>
		/// Only when you enabled `useParticleSystem` in <see cref="particlePrefeb"/>,
		/// you can call this function to get trigger colliders for particles.<para/>
		/// For particle based on <see cref="GameObject"/>, all colliders in the sence are
		/// able to collide.
		/// </summary>
		/// <param name="index">Collider index.</param>
		public void GetTriggerColloder(int index)
		{
			((PSParticleSystem)particleSystem).GetTriggerCollider(index);
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
		private IParticleSystem particleSystem;
		private ParticlePrefeb particlePrefeb;
	}
}
