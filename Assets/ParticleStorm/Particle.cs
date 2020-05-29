using ParticleStorm.Core;
using ParticleStorm.Factories;
using ParticleStorm.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleStorm
{
	/// <summary>
	/// <para>Particle is the bisic class of particle storm.</para>
	/// <para>A particle can be emitted, controlled, and add modules.</para>
	/// </summary>
	public class Particle : IParticle
	{
		public string Name { get => name; set { Register(value); } }

		public Particle() { InitParticleSystem(); }

		public Particle(string name) { Register(name); InitParticleSystem(); }
		
		public Particle(ParticlePrefeb prefeb)
		{
			InitParticleSystem();
			SetPrefeb(prefeb);
			Register(prefeb.name);
		}

		public Particle(string name, ParticlePrefeb prefeb)
		{
			InitParticleSystem();
			SetPrefeb(prefeb);
			Register(name);
		}

		~Particle()
		{
			if (Name != null) { Dict.Remove(name); }
			if (particlePrefeb != null) { ParticlePrefeb.Destroy(particlePrefeb); }
		}

		/// <summary>
		/// Find a particle by name.
		/// </summary>
		/// <param name="name">Particle name.</param>
		/// <returns></returns>
		/// <exception cref="KeyNotFoundException"/>
		public static Particle Find(string name)
		{
			if (Dict.TryGetValue(name, out Particle particle))
				return particle;
			else
				throw new KeyNotFoundException("Can't find particle " + name);
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
			if (particlePrefeb.useParticleSystem)
			{
				((PSParticleSystem)particleSystem).SetTriggerCollider(index, collider);
			}
			else
			{
				throw new InvalidOperationException("Only particle system based particle need setting colloders.");
			}
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
			if (particlePrefeb.useParticleSystem)
			{
				((PSParticleSystem)particleSystem).GetTriggerCollider(index);
			}
			else
			{
				throw new InvalidOperationException("Only particle system based particle can get colloders.");
			}
		}

		/// <summary>
		/// Emit particles.
		/// </summary>
		/// <param name="emitParams">Initial parameters of the emitted particle.</param>
		/// <param name="num">Emit number.</param>
		public void Emit(EmitParams emitParams, int num) => particleSystem.Emit(emitParams, num);

		/// <summary>
		/// Name the particle and register itself into <see cref="Dict"/>.
		/// </summary>
		/// <param name="name">Particle name.</param>
		private void Register(string name)
		{
			if (name == null)
			{
				Debug.LogError("Particle name can't be null");
			}
			else if (Dict.ContainsKey(name))
			{
				Debug.LogError("Particle " + name + " already exists.");
			}
			else
			{
				if (this.name != null)
					Dict.Remove(this.name);
				Dict.Add(name, this);
				this.name = name;
			}
		}

		/// <summary>
		/// Initialize the particle.
		/// </summary>
		private void InitParticleSystem()
		{
			if (particlePrefeb == null || particlePrefeb.useParticleSystem)
			{
				particleSystem = ParticleSystemFactory.PSParticleSystem();
			}
			else
			{
				throw new NotImplementedException("Game object particle not implemented yet.");
			}
		}

		/// <summary>
		/// Dictionary for all named particles.
		/// </summary>
		private static readonly Dictionary<string, Particle> Dict = new Dictionary<string, Particle>();
		
		/// <summary>
		/// Particle system object, can be realized with <see cref="ParticleSystem"/> or <see cref="GameObject"/>.
		/// </summary>
		private IParticleSystem particleSystem;
		private ParticlePrefeb particlePrefeb;
		private string name;
	}
}
