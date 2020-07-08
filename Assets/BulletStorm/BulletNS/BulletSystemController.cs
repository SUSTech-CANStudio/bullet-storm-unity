using BulletStorm.BulletNS.Script;
using System.Collections.Generic;
using UnityEngine;

namespace BulletStorm.BulletNS
{
	/// <summary>
	/// Contains a <see cref="UnityEngine.ParticleSystem"/> and can emit particle from it.<para/>
	/// There two kinds of <see cref="BulletSystemController"/>s,
	/// one called origin, the other called copy.<para/>
	/// Origin is in <see cref="Bullet"/>, one kind of particle
	/// only has one origin. Origin is always at (0, 0, 0) and never rotate.<para/>
	/// Copy can be created from <see cref="Bullet.GetCopy(Transform)"/>,
	/// and the <see cref="GameObject"/> in copies are children of given <see cref="Transform"/>.
	/// </summary>
	public class BulletSystemController
	{
		public bool IsOrigin { get; private set; }
		public GameObject GameObject { get; private set; }
		public ParticleSystem ParticleSystem { get; private set; }
		public UpdateEvent UpdateEvent { get; set; }
		public CollisionEvent CollisionEvent { get; set; }
		public EmissionEvent EmissionEvent { get; set; }

		private BulletStatusList particles;
		private List<ParticleCollisionEvent> collisionEvents;

		/// <summary>
		/// Create an origin <see cref="BulletSystemController"/>.
		/// The <see cref="UnityEngine.ParticleSystem"/> will be located at origin point.
		/// </summary>
		public BulletSystemController()
		{
			IsOrigin = true;
			GameObject = new GameObject();
			Initialize();
		}

		/// <summary>
		/// Create an origin <see cref="BulletSystemController"/>.
		/// The <see cref="UnityEngine.ParticleSystem"/> will be located at origin point.
		/// </summary>
		/// <param name="name">Name of the particle system game objecct.</param>
		public BulletSystemController(string name) : this() => GameObject.name = name;

		/// <summary>
		/// Create an origin <see cref="BulletSystemController"/>.
		/// The <see cref="UnityEngine.ParticleSystem"/> will be located at origin point.
		/// </summary>
		/// <param name="prefeb">The particle prefeb</param>
		public BulletSystemController(BulletPrefeb prefeb) : this(prefeb.name)
			=> prefeb.ApplicateOn(this);

		/// <summary>
		/// Create an origin <see cref="BulletSystemController"/>.
		/// The <see cref="UnityEngine.ParticleSystem"/> will be located at origin point.
		/// </summary>
		/// <param name="name">Name of the particle system game objecct.</param>
		/// <param name="prefeb">The particle prefeb</param>
		public BulletSystemController(string name, BulletPrefeb prefeb) : this(prefeb)
			=> GameObject.name = name;

		/// <summary>
		/// Copy an origin.
		/// </summary>
		/// <param name="origin">The origin particle system controller</param>
		public BulletSystemController(BulletSystemController origin, Transform parent)
		{
			IsOrigin = false;
			GameObject = GameObject.Instantiate<GameObject>(origin.GameObject, parent, false);
			Initialize();
			UpdateEvent = origin.UpdateEvent;
			CollisionEvent = origin.CollisionEvent;
		}

		~BulletSystemController()
		{
			GameObject.Destroy(GameObject);
		}

		/// <summary>
		/// Emit a particle.
		/// </summary>
		/// <param name="emitParams">The particle parameters when emitting</param>
		public void Emit(EmitParams emitParams)
		{
			ParticleSystem.Emit(emitParams.Full, 1);
		}

		/// <summary>
		/// Should be called on update.
		/// </summary>
		public void Update()
		{
			if (UpdateEvent != null && UpdateEvent.OnParticleUpdate != null)
			{
				particles.Update(UpdateEvent.OnParticleUpdate, UpdateEvent.ParallelOnUpdate);
			}
		}

		/// <summary>
		/// Should be called on fixed update.
		/// </summary>
		public void FixedUpdate()
		{
			if (UpdateEvent != null && UpdateEvent.OnParticleFixedUpdate != null)
			{
				particles.Update(UpdateEvent.OnParticleFixedUpdate, UpdateEvent.ParallelOnFixedUpdate);
			}
		}

		/// <summary>
		/// Should be called on late update.
		/// </summary>
		public void LateUpdate()
		{
			if (UpdateEvent != null && UpdateEvent.OnParticleLateUpdate != null)
			{
				particles.Update(UpdateEvent.OnParticleLateUpdate, UpdateEvent.ParallelOnLateUpdate);
			}
		}

		/// <summary>
		/// Should be called on particle collidion.
		/// </summary>
		/// <param name="other">The collided game object</param>
		public void OnParticleCollision(GameObject other)
		{
			if (CollisionEvent != null && CollisionEvent.OnGameObjectCollision != null)
			{
				ParticleSystem.GetCollisionEvents(other, collisionEvents);
				CollisionEvent.OnGameObjectCollision(other, collisionEvents);
			}
		}

		private void Initialize()
		{
			if (IsOrigin)
			{
				// Create particle system
				ParticleSystem = GameObject.AddComponent<ParticleSystem>();

				// Add updater
				var updater = GameObject.AddComponent<BulletSystemControllerUpdater>();
				updater.UpdateFor(this);

				// Enable GPU
				var psr = GameObject.GetComponent<ParticleSystemRenderer>();
				psr.enableGPUInstancing = true;

				// Disable emission module
				var emission = ParticleSystem.emission;
				emission.enabled = false;

				// Lock transform
				GameObject.transform.position = Vector3.zero;
				GameObject.transform.rotation = Quaternion.identity;
				GameObject.isStatic = true;
			}
			else
			{
				ParticleSystem = GameObject.GetComponent<ParticleSystem>();
				GameObject.GetComponent<BulletSystemControllerUpdater>().UpdateFor(this);
				GameObject.isStatic = false;
			}

			// Init lists
			particles = new BulletStatusList(ParticleSystem);
			collisionEvents = new List<ParticleCollisionEvent>();
		}
	}
}
