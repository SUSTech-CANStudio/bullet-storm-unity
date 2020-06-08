using ParticleStorm.Core;
using ParticleStorm.Util;
using System;
using UnityEngine;

namespace ParticleStorm
{
	/// <summary>
	/// <para>Particle is the bisic class of particle storm.</para>
	/// <para>A particle can be emitted, controlled, and add modules.</para>
	/// </summary>
	public class Particle : Named<Particle>
	{
		/// <summary>
		/// The origin particle system controller of this particle.
		/// </summary>
		public ParticleSystemController Origin { get; private set; }

		public Particle() => Origin = new ParticleSystemController();

		public Particle(string name) { Name = name; Origin = new ParticleSystemController(name); }
		
		public Particle(ParticlePrefeb prefeb)
		{
			Origin = new ParticleSystemController(prefeb);
			Name = prefeb.name;
		}

		public Particle(string name, ParticlePrefeb prefeb)
		{
			Origin = new ParticleSystemController(name, prefeb);
			Name = name;
		}

		/// <summary>
		/// Set a <see cref="ParticlePrefeb"/> to the particle.
		/// </summary>
		/// <param name="prefeb">The particle prefeb.</param>
		/// <exception cref="ArgumentNullException"/>
		public void SetPrefeb(ParticlePrefeb prefeb)
		{
			if (prefeb is null) { throw new ArgumentNullException(nameof(prefeb)); }
			prefeb.ApplicateOn(Origin);
		}

		/// <summary>
		/// Get a copy of the origin <see cref="ParticleSystemController"/>.
		/// </summary>
		/// <param name="parent">Parent transform of the copy</param>
		/// <returns></returns>
		internal ParticleSystemController GetCopy(Transform parent) =>
			new ParticleSystemController(Origin, parent);
	}
}
