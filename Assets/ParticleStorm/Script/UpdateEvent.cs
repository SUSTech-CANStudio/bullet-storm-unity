using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticleStorm.Util;

namespace ParticleStorm.Script
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

		public UpdateEvent(string name, ParticleUpdateScript script)
		{
			Name = name;
			OnParticleUpdate = script;
		}
	}
}
