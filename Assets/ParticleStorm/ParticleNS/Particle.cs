using ParticleStorm.Util;
using System;
using UnityEngine;

namespace ParticleStorm.ParticleNS
{
	/// <summary>
	/// Represents one kind of particle. Particles can be emitted by
	/// <see cref="ParticleSystemController"/>s, every kind of particle
	/// has one <see cref="Origin"/> <see cref="ParticleSystemController"/>.
	/// </summary>
	public class Particle : Named<Particle>
	{
		/// <summary>
		/// The origin particle system controller of this particle.
		/// </summary>
		public ParticleSystemController Origin { get; private set; }

		/// <summary>
		/// Create a particle.
		/// </summary>
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
		/// Add a component to the <see cref="ParticleSystem"/> game object of
		/// this particle. So that you can use <see cref="GameObject.GetComponent(Type)}"/>
		/// to get it when the particle collides with a game object and calling
		/// <c>OnParticleCollision</c>.
		/// </summary>
		/// <param name="componentType"></param>
		public void AddComponent(Type componentType) => Origin.GameObject.AddComponent(componentType);

		public void AddComponent<T>() where T : Component => Origin.GameObject.AddComponent<T>();

		/// <summary>
		/// Get a copy of the origin <see cref="ParticleSystemController"/>.
		/// </summary>
		/// <param name="parent">Parent transform of the copy</param>
		/// <returns></returns>
		internal ParticleSystemController GetCopy(Transform parent) =>
			new ParticleSystemController(Origin, parent);
	}
}
