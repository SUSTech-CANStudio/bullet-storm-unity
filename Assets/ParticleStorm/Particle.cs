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
		public string name { get => m_Name; set { Register(value); } }

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
			if (name != null) dict.Remove(m_Name);
			if (m_ParticlePrefeb != null) ParticlePrefeb.Destroy(m_ParticlePrefeb);
		}

		/// <summary>
		/// Find a particle by name.
		/// </summary>
		/// <param name="name">Particle name.</param>
		/// <returns></returns>
		/// <exception cref="KeyNotFoundException"/>
		public static Particle Find(string name)
		{
			if (dict.TryGetValue(name, out Particle particle))
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
				throw new ArgumentNullException(nameof(prefeb), "Particle prefeb can not be null.");
			if (m_ParticlePrefeb != null && destroy)
				ParticlePrefeb.Destroy(m_ParticlePrefeb);
			m_ParticlePrefeb = prefeb;
			m_ParticlePrefeb.Bind(m_ParticleSystem);
		}

		/// <summary>
		/// Emit particles.
		/// </summary>
		/// <param name="emitParams">Initial parameters of the emitted particle.</param>
		/// <param name="num">Emit number.</param>
		public void Emit(EmitParams emitParams, int num) => m_ParticleSystem.Emit(emitParams, num);

		/// <summary>
		/// Name the particle and register itself into <see cref="dict"/>.
		/// </summary>
		/// <param name="name">Particle name.</param>
		private void Register(string name)
		{
			if (name == null)
				Debug.LogError("Particle name can't be null");
			else if (dict.ContainsKey(name))
				Debug.LogError("Particle " + name + " already exists.");
			else
			{
				if (m_Name != null)
					dict.Remove(m_Name);
				dict.Add(name, this);
				m_Name = name;
			}
		}

		/// <summary>
		/// Initialize the particle.
		/// </summary>
		private void InitParticleSystem()
		{
			if (m_ParticlePrefeb == null || m_ParticlePrefeb.useParticleSystem)
				m_ParticleSystem = ParticleSystemFactory.PSParticleSystem();
			else
				throw new NotImplementedException("Game object particle not implemented yet.");

		}

		/// <summary>
		/// Dictionary for all named particles.
		/// </summary>
		private static readonly Dictionary<string, Particle> dict = new Dictionary<string, Particle>();
		
		/// <summary>
		/// Particle system object, can be realized with <see cref="ParticleSystem"/> or <see cref="GameObject"/>.
		/// </summary>
		private IParticleSystem m_ParticleSystem;
		private ParticlePrefeb m_ParticlePrefeb;
		private string m_Name;
	}
}
