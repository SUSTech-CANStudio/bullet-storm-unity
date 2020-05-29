using UnityEngine;

namespace ParticleStorm.Script
{
	/// <summary>
	/// Status of a single particle.
	/// </summary>
	public class ParticleStatus
	{
		/// <summary>
		/// In parallel mode, calling <see cref="Time.deltaTime"/> will cause exception, use this instead.
		/// </summary>
		public float DeltaTime => statusList.DeltaTime;
		/// <summary>
		/// The position of the particle.
		/// </summary>
		public Vector3 Position { get; set; }
		/// <summary>
		/// The velocity of the particle.
		/// </summary>
		public Vector3 Velocity { get; set; }
		/// <summary>
		/// The euler rotation of the particle.
		/// </summary>
		public Vector3 Rotation { get; set; }
		/// <summary>
		/// The euler angular velocity of the particle.
		/// </summary>
		public Vector3 AngularVelocity { get; set; }
		/// <summary>
		/// The remaining lifetime of the particle.
		/// </summary>
		public float RemainingLifetime { get; set; }
		/// <summary>
		/// The starting lifetime of the particle.
		/// </summary>
		public float StartLifetime { get; set; }
		/// <summary>
		/// Calculate the current color of the particle.
		/// </summary>
		public Color32 Color { get => particle.GetCurrentColor(statusList.ParticleSystem); }
		/// <summary>
		/// The initial color of the particle.
		/// The current color of the particle is calculated
		/// procedurally based on this value and the active
		/// color modules.
		/// </summary>
		public Color32 StartColor { get; set; }
		/// <summary>
		/// Calculate the current size of the particle.
		/// </summary>
		public float Size { get => particle.GetCurrentSize(statusList.ParticleSystem); }
		/// <summary>
		/// The initial size of the particle.
		/// The current size of the particle is calculated
		/// procedurally based on this value and the active
		/// size modules.
		/// </summary>
		public float StartSize { get; set; }
		/// <summary>
		/// Mesh index of the particle,
		/// used for choosing which Mesh a particle is rendered with.
		/// </summary>
		public int MeshIndex
		{
			get => particle.GetMeshIndex(statusList.ParticleSystem);
			set => particle.SetMeshIndex(value);
		}

		internal ParticleStatus(ref ParticleSystem.Particle particle, ParticleStatusList statusList)
		{
			Position = particle.position;
			Velocity = particle.velocity;
			Rotation = particle.rotation3D;
			AngularVelocity = particle.angularVelocity3D;
			RemainingLifetime = particle.remainingLifetime;
			StartLifetime = particle.startLifetime;
			StartColor = particle.startColor;
			StartSize = particle.startSize;
			this.particle = particle;
			this.statusList = statusList;
		}

		internal void ToParticle(ref ParticleSystem.Particle particle)
		{
			particle.position = Position;
			particle.velocity = Velocity;
			particle.rotation3D = Rotation;
			particle.angularVelocity3D = AngularVelocity;
			particle.remainingLifetime = RemainingLifetime;
			particle.startLifetime = StartLifetime;
			particle.startColor = StartColor;
			particle.startSize = StartSize;
		}

		internal ParticleSystem.Particle ToParticle()
		{
			var particle = new ParticleSystem.Particle();
			ToParticle(ref particle);
			return particle;
		}

		private ParticleSystem.Particle particle;
		private readonly ParticleStatusList statusList;
	}
}
