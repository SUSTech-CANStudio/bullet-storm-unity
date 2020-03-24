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
		void Emit(EmitParams emitParams, int num);
	}
}
