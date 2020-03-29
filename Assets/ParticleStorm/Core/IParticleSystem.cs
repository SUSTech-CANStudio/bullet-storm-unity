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
		/// Add a module to the particle system.
		/// </summary>
		/// <param name="module"></param>
		void AddModule(IParticleModule module);

		/// <summary>
		/// Delete a module from the particle system.
		/// </summary>
		/// <param name="module"></param>
		void DeleteModule(IParticleModule module);
	}
}
