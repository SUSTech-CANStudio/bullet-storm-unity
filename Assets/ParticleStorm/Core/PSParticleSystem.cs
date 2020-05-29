using ParticleStorm.Modules;
using ParticleStorm.Script;
using ParticleStorm.Util;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleStorm.Core
{
	/// <summary>
	/// Particle system based on <see cref="ParticleSystem"/>.
	/// </summary>
	internal class PSParticleSystem : MonoBehaviour, IParticleSystem
	{
		/// <summary>
		/// Main particle system settings.
		/// </summary>
		public ParticleSystem.MainModule Main { get => ps.main; }

		#region modules
		internal ScriptModule scriptModule;
		internal CollisionModule collisionModule;
		#endregion

		public void Emit(EmitParams emitParams, int num) => ps.Emit(emitParams.Full, num);
		public void ApplicateModule(IParticleModule module) => module.ApplicateOn(this);
		public void SetTriggerCollider(int index, Collider collider) => ps.trigger.SetCollider(index, collider);
		public Component GetTriggerCollider(int index) => ps.trigger.GetCollider(index);

		private void Awake()
		{
			// Add component
			if (!TryGetComponent(out ps))
				ps = gameObject.AddComponent<ParticleSystem>();
			// Disable auto generate
			ps.Stop();
			// Enable GPU
			var psr = ps.GetComponent<ParticleSystemRenderer>();
			psr.enableGPUInstancing = true;
			// Lock location
			ps.transform.position = Vector3.zero;
			ps.transform.rotation = Quaternion.identity;
			ps.gameObject.isStatic = true;
			// Init particle list
			particles = new ParticleStatusList(ps);
		}

		private void Update()
		{
			if (scriptModule.enabled && scriptModule.updateScript != null)
			{
				particles.Update(scriptModule.updateScript, scriptModule.parallelUpdate);
			}
		}

		private void FixedUpdate()
		{
			if (scriptModule.enabled && scriptModule.fixedUpdateScript != null)
			{
				particles.Update(scriptModule.fixedUpdateScript, scriptModule.parallelFixedUpdate);
			}
		}

		private void LateUpdate()
		{
			if (scriptModule.enabled && scriptModule.lateUpdateScript != null)
			{
				particles.Update(scriptModule.lateUpdateScript, scriptModule.parallelLateUpdate);
			}
		}

		private void OnParticleCollision(GameObject other)
		{
			if (collisionModule.enabled)
			{
				// Set trigger colliders
				ps.trigger.SetCollider(nextColliderIndex, other.GetComponent<Collider>());
				nextColliderIndex = (nextColliderIndex + 1) % ps.trigger.maxColliderCount;
				// Collider script
				CollisionEvent collisionScript = collisionModule.onCollision;
				if (collisionScript.OnGameObjectCollision != null)
				{
					ps.GetCollisionEvents(other, collisionEvents);
					collisionScript.OnGameObjectCollision(other, collisionEvents);
				}
			}
		}

		private void OnParticleTrigger()
		{
			if (collisionModule.enabled && collisionModule.onCollision.OnParticleCollision != null)
			{
				particles.Trigger(collisionModule.onCollision.OnParticleCollision, collisionModule.triggerType);
			}
		}

		private ParticleSystem ps;
		private ParticleStatusList particles;
		private int nextColliderIndex;
		private readonly List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
	}
}
