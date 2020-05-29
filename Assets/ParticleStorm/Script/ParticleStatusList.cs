using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ParticleStorm.Script
{
	internal class ParticleStatusList
	{
		public ParticleSystem ParticleSystem { get; }
		public float DeltaTime { get; private set; }
		
		public ParticleStatusList(ParticleSystem particleSystem)
		{
			ParticleSystem = particleSystem;
			particleStatuses = new List<ParticleStatus>();
		}

		public void Update(ParticleUpdateScript script, bool parallel)
		{
			int readNum;
			readNum = ReadParticles();
			DeltaTime = Time.deltaTime;
			if (parallel)
			{
				Parallel.ForEach(particleStatuses, (particle) => { script(particle); });
			}
			else
			{
				foreach (ParticleStatus particle in particleStatuses) script(particle);
			}
			WriteParticles(readNum);
		}

		public void Trigger(ParticleCollisionScript script, ParticleSystemTriggerEventType eventType)
		{
			// Get
			List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
			int num = ParticleSystem.GetTriggerParticles(eventType, particles);
			// Script
			for (int i = 0; i < num; i++)
			{
				ParticleSystem.Particle particle = particles[i];
				ParticleStatus status = new ParticleStatus(ref particle, this);
				script(status);
				status.ToParticle(ref particle);
				particles[i] = particle;
			}
			// Set
			ParticleSystem.SetTriggerParticles(eventType, particles);
		}

		private int ReadParticles()
		{
			UpdateBufferSize();
			int num = ParticleSystem.GetParticles(particles);
			particleStatuses.Clear();
			for (int i = 0; i < num; i++)
				particleStatuses.Add(new ParticleStatus(ref particles[i], this));
			return num;
		}

		private void WriteParticles(int num)
		{
			for (int i = 0; i < num; i++)
				particleStatuses[i].ToParticle(ref particles[i]);
			ParticleSystem.SetParticles(particles, num);
		}

		private void UpdateBufferSize()
		{
			if (particles == null || particles.Length < ParticleSystem.main.maxParticles)
				particles = new ParticleSystem.Particle[ParticleSystem.main.maxParticles];
		}

		private readonly List<ParticleStatus> particleStatuses;
		private ParticleSystem.Particle[] particles;
	}
}
