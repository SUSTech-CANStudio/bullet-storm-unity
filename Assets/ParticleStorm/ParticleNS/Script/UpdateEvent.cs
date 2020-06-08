using ParticleStorm.Util;
using System.ComponentModel;

namespace ParticleStorm.ParticleNS.Script
{
	/// <summary>
	/// Script for particles.
	/// </summary>
	/// <param name="particle">The particle status, you can get and modify it in your script.</param>
	public delegate void ParticleUpdateScript(ParticleStatus particle);

	/// <summary>
	/// On update event for particle collision module.
	/// </summary>
	public class UpdateEvent : Named<UpdateEvent>
	{
		/// <summary>
		/// Script for the particle on update.
		/// </summary>
		public ParticleUpdateScript OnParticleUpdate { get; set; }

		/// <summary>
		/// Script for the particle on fixed update.
		/// </summary>
		public ParticleUpdateScript OnParticleFixedUpdate { get; set; }

		/// <summary>
		/// Script for the particle on late update.
		/// </summary>
		public ParticleUpdateScript OnParticleLateUpdate { get; set; }

		public bool ParallelOnUpdate { get; set; }
		public bool ParallelOnFixedUpdate { get; set; }
		public bool ParallelOnLateUpdate { get; set; }

		public UpdateEvent(string name) => Name = name;

		/// <summary>
		/// Create an update event with a script execute on update.
		/// </summary>
		/// <param name="name">Update event name</param>
		/// <param name="script">The script function</param>
		public UpdateEvent(string name, ParticleUpdateScript script) : this(name, script, 0) { }

		/// <summary>
		/// Create an update event with a script execute on update, fixed update, or late update.
		/// </summary>
		/// <param name="name">Update event name</param>
		/// <param name="script">The script function</param>
		/// <param name="mode">0 for update, 1 for fixed update, 2 for late update</param>
		public UpdateEvent(string name, ParticleUpdateScript script, int mode)
		{
			Name = name;
			switch (mode)
			{
				case 0:
					OnParticleUpdate = script;
					break;
				case 1:
					OnParticleFixedUpdate = script;
					break;
				case 2:
					OnParticleLateUpdate = script;
					break;
				default:
					throw new InvalidEnumArgumentException();
			}
		}
	}
}
