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
	/// Abstract game object particle.
	/// </summary>
	class AOParticle : MonoBehaviour, IParticle
	{
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
