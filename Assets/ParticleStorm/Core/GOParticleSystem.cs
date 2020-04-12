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
	/// Particle system based on <see cref="GameObject"/>.
	/// </summary>
	class GOParticleSystem : MonoBehaviour, IParticleSystem
	{
		public void ApplicateModule(IParticleModule module)
		{
			throw new NotImplementedException();
		}

		public void Emit(EmitParams emitParams, int num)
		{
			throw new NotImplementedException();
		}

		public void Init(string name)
		{
			throw new NotImplementedException();
		}
	}
}
