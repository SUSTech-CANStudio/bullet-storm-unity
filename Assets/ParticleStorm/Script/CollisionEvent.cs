using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ParticleStorm.Script
{
	/// <summary>
	/// Script for game object when it collides with particles.
	/// </summary>
	/// <param name="gameObject">The game object that collides.</param>
	/// <param name="collisionEvents">Infomations of collisions.</param>
	public delegate void GameObjectCollisionScript(GameObject gameObject, List<ParticleCollisionEvent> collisionEvents);
	
	/// <summary>
	/// Script for particle when it collides and goes into a game object.
	/// </summary>
	/// <param name="particle">The particle that collides.</param>
	public delegate void ParticleCollisionScript(ParticleStatus particle);

	/// <summary>
	/// On collision event for particle collision module.
	/// </summary>
	public class CollisionEvent
	{
		/// <summary>
		/// Name of the CollisionEvent.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Script for the game object in collision.
		/// </summary>
		public GameObjectCollisionScript OnGameObjectCollision { get; set; }
		/// <summary>
		/// Script for particles in collision.
		/// </summary>
		public ParticleCollisionScript OnParticleCollision { get; set; }

		public CollisionEvent(string name, GameObjectCollisionScript scriptForGameObject)
		{
			Name = name;
			OnGameObjectCollision = scriptForGameObject;
		}

		public CollisionEvent(string name, ParticleCollisionScript scriptForParticle)
		{
			Name = name;
			OnParticleCollision = scriptForParticle;
		}

		public CollisionEvent(string name, GameObjectCollisionScript scriptForGameObject, ParticleCollisionScript scriptForParticle)
		{
			Name = name;
			OnGameObjectCollision = scriptForGameObject;
			OnParticleCollision = scriptForParticle;
		}
	}
}
