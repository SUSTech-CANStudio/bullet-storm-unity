using ParticleStorm.Util;
using ParticleStorm.Modules;
using ParticleStorm.Script;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		public ParticleSystem.MainModule main { get => ps.main; }

		#region modules
		public ScriptModule scriptModule;
		#endregion

		public void Emit(EmitParams emitParams, int num) => ps.Emit(emitParams.full, num);
		public void ApplicateModule(IParticleModule module) => module.ApplicateOn(this);

		private void InitializeIfNeeded()
		{
			if (ps == null)
			{
				if (!TryGetComponent(out ps))
					ps = gameObject.AddComponent<ParticleSystem>();

				psr = ps.GetComponent<ParticleSystemRenderer>();

				// Disable auto generate
				ps.Stop();
				// Enable GPU
				psr.enableGPUInstancing = true;
				// Lock location
				ps.transform.position = Vector3.zero;
				ps.transform.rotation = Quaternion.identity;
				ps.gameObject.isStatic = true;
			}

			if (m_Particles == null || m_Particles.Length < ps.main.maxParticles)
				m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
		}

		private void InvokeScript(ParticleScript.Script script, List<ParticleStatus> particles, bool parallel)
		{
			if (parallel)
				Parallel.ForEach(particles, (particle) => { script(particle); });
			else
				foreach (ParticleStatus particle in particles) script(particle);
		}

		private void Awake()
		{
			InitializeIfNeeded();
		}

		private void Update()
		{
			if (scriptModule != null && scriptModule.enabled && scriptModule.updateScript != null)
			{
				InitializeIfNeeded();
				// Get particles
				List<ParticleStatus> particles = new List<ParticleStatus>();
				int num = ps.GetParticles(m_Particles);
				for (int i = 0; i < num; i++)
					particles.Add(new ParticleStatus(m_Particles[i], ps));
				// Invoke
				InvokeScript(scriptModule.updateScript, particles, scriptModule.parallelUpdate);
				// Set particles
				for (int i = 0; i < num; i++)
					particles[i].ToParticle(ref m_Particles[i]);
				ps.SetParticles(m_Particles);
			}
		}

		private void FixedUpdate()
		{
			if (scriptModule != null && scriptModule.enabled && scriptModule.fixedUpdateScript != null)
			{
				InitializeIfNeeded();
				// Get particles
				List<ParticleStatus> particles = new List<ParticleStatus>();
				int num = ps.GetParticles(m_Particles);
				for (int i = 0; i < num; i++)
					particles.Add(new ParticleStatus(m_Particles[i], ps));
				// Invoke
				InvokeScript(scriptModule.fixedUpdateScript, particles, scriptModule.parallelFixedUpdate);
				// Set particles
				for (int i = 0; i < num; i++)
					particles[i].ToParticle(ref m_Particles[i]);
				ps.SetParticles(m_Particles);
			}
		}

		private void LateUpdate()
		{
			if (scriptModule != null && scriptModule.enabled && scriptModule.lateUpdateScript != null)
			{
				InitializeIfNeeded();
				// Get particles
				List<ParticleStatus> particles = new List<ParticleStatus>();
				int num = ps.GetParticles(m_Particles);
				for (int i = 0; i < num; i++)
					particles.Add(new ParticleStatus(m_Particles[i], ps));
				// Invoke
				InvokeScript(scriptModule.lateUpdateScript, particles, scriptModule.parallelLateUpdate);
				// Set particles
				for (int i = 0; i < num; i++)
					particles[i].ToParticle(ref m_Particles[i]);
				ps.SetParticles(m_Particles);
			}
		}

		private void OnParticleTrigger()
		{
			
		}

		private ParticleSystem ps;
		private ParticleSystemRenderer psr;
		private ParticleSystem.Particle[] m_Particles;
	}
}
