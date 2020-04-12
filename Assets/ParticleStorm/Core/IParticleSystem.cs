using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleStorm.Core
{
	interface IParticleSystem : IParticle
	{
		/// <summary>
		/// Applicate a module to the particle system.
		/// </summary>
		/// <param name="module"></param>
		void ApplicateModule(IParticleModule module);
	}
}
