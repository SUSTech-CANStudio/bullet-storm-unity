using BulletStorm.Util;
using System;
using UnityEngine;

namespace BulletStorm.BulletNS
{
	/// <summary>
	/// Represents one kind of particle. Particles can be emitted by
	/// <see cref="BulletSystemController"/>s, every kind of particle
	/// has one <see cref="Origin"/> <see cref="BulletSystemController"/>.
	/// </summary>
	public class Bullet : Named<Bullet>
	{
		/// <summary>
		/// The origin particle system controller of this particle.
		/// </summary>
		public BulletSystemController Origin { get; private set; }

		/// <summary>
		/// Create a particle.
		/// </summary>
		public Bullet() => Origin = new BulletSystemController();

		public Bullet(string name) { Name = name; Origin = new BulletSystemController(name); }

		public Bullet(BulletPrefeb prefeb)
		{
			Origin = new BulletSystemController(prefeb);
			Name = prefeb.name;
		}

		public Bullet(string name, BulletPrefeb prefeb)
		{
			Origin = new BulletSystemController(name, prefeb);
			Name = name;
		}

		/// <summary>
		/// Set a <see cref="BulletPrefeb"/> to the particle.
		/// </summary>
		/// <param name="prefeb">The particle prefeb.</param>
		/// <exception cref="ArgumentNullException"/>
		public void SetPrefeb(BulletPrefeb prefeb)
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
		/// Get a copy of the origin <see cref="BulletSystemController"/>.
		/// </summary>
		/// <param name="parent">Parent transform of the copy</param>
		/// <returns></returns>
		internal BulletSystemController GetCopy(Transform parent) =>
			new BulletSystemController(Origin, parent);
	}
}
