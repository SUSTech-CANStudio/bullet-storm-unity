using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Util;

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

		public void Awake()
		{
			InitializeIfNeeded();
		}

		public void Emit(EmitParams emitParams, int num) => ps.Emit(emitParams.full, num);
		public void AddModule(IParticleModule module) => module.AddOn(this);
		public void DeleteModule(IParticleModule module) => module.DeleteFrom(this);

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

		private ParticleSystem ps;
		private ParticleSystemRenderer psr;
		private ParticleSystem.Particle[] m_Particles;
	}
}
