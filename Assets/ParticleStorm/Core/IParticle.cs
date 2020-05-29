using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Util;

namespace ParticleStorm.Core
{
	public interface IParticle
	{
		/// <summary>
		/// Emit the particle.
		/// </summary>
		/// <param name="emitParams">Parameters for emission.</param>
		/// <param name="num">Particle number.</param>
		void Emit(EmitParams emitParams, int num);
	}
}
