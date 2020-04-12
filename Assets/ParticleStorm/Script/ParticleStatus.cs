using UnityEngine;

namespace ParticleStorm.Script
{
	public class ParticleStatus
	{
		/// <summary>
		/// The position of the particle.
		/// </summary>
		public Vector3 position;
		/// <summary>
		/// The velocity of the particle.
		/// </summary>
		public Vector3 velocity;
		/// <summary>
		/// The euler rotation of the particle.
		/// </summary>
		public Vector3 rotation;
		/// <summary>
		/// The euler angular velocity of the particle.
		/// </summary>
		public Vector3 angularVelocity;
		/// <summary>
		/// The remaining lifetime of the particle.
		/// </summary>
		public float remainingLifetime;
		/// <summary>
		/// The starting lifetime of the particle.
		/// </summary>
		public float startLifetime;
		/// <summary>
		/// Calculate the current color of the particle.
		/// </summary>
		public Color32 color { get => m_Particle.GetCurrentColor(m_ParticleSystem); }
		/// <summary>
		/// The initial color of the particle.
		/// The current color of the particle is calculated
		/// procedurally based on this value and the active
		/// color modules.
		/// </summary>
		public Color32 startColor;
		/// <summary>
		/// Calculate the current size of the particle.
		/// </summary>
		public float size { get => m_Particle.GetCurrentSize(m_ParticleSystem); }
		/// <summary>
		/// The initial size of the particle.
		/// The current size of the particle is calculated
		/// procedurally based on this value and the active
		/// size modules.
		/// </summary>
		public float startSize;
		/// <summary>
		/// Mesh index of the particle,
		/// used for choosing which Mesh a particle is rendered with.
		/// </summary>
		public int meshIndex
		{
			get => m_Particle.GetMeshIndex(m_ParticleSystem);
			set => m_Particle.SetMeshIndex(value);
		}

		internal ParticleStatus(ParticleSystem.Particle particle, ParticleSystem particleSystem)
		{
			position = particle.position;
			velocity = particle.velocity;
			rotation = particle.rotation3D;
			angularVelocity = particle.angularVelocity3D;
			remainingLifetime = particle.remainingLifetime;
			startLifetime = particle.startLifetime;
			startColor = particle.startColor;
			startSize = particle.startSize;
			m_Particle = particle;
			m_ParticleSystem = particleSystem;
		}

		internal void ToParticle(ref ParticleSystem.Particle particle)
		{
			particle.position = position;
			particle.velocity = velocity;
			particle.rotation3D = rotation;
			particle.angularVelocity3D = angularVelocity;
			particle.remainingLifetime = remainingLifetime;
			particle.startLifetime = startLifetime;
			particle.startColor = startColor;
			particle.startSize = startSize;
		}

		private ParticleSystem.Particle m_Particle;
		private ParticleSystem m_ParticleSystem;
	}
}
