using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BulletStorm.BulletNS.Script
{
	internal class BulletStatusList
	{
		public ParticleSystem ParticleSystem { get; }
		public float DeltaTime { get; private set; }

		public BulletStatusList(ParticleSystem particleSystem)
		{
			ParticleSystem = particleSystem;
			particleStatuses = new List<BulletStatus>();
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
				foreach (BulletStatus particle in particleStatuses) { script(particle); }
			}
			WriteParticles(readNum);
		}

		public void Collision(ParticleCollisionScript script, ParticleSystemTriggerEventType eventType)
		{
			// Get
			List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
			int num = ParticleSystem.GetTriggerParticles(eventType, particles);
			// Script
			for (int i = 0; i < num; i++)
			{
				ParticleSystem.Particle particle = particles[i];
				BulletStatus status = new BulletStatus(ref particle, this);
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
			{
				particleStatuses.Add(new BulletStatus(ref particles[i], this));
			}
			return num;
		}

		private void WriteParticles(int num)
		{
			for (int i = 0; i < num; i++) { particleStatuses[i].ToParticle(ref particles[i]); }
			ParticleSystem.SetParticles(particles, num);
		}

		private void UpdateBufferSize()
		{
			if (particles == null || particles.Length < ParticleSystem.main.maxParticles)
			{
				particles = new ParticleSystem.Particle[ParticleSystem.main.maxParticles];
			}
		}

		private readonly List<BulletStatus> particleStatuses;
		private ParticleSystem.Particle[] particles;
	}
}
