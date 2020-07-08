using BulletStorm.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BulletStorm.BulletNS.Script
{
	public delegate void ParticleEmissionScript(Transform emitter, EmitParams emitParams);

	public class EmissionEvent : Named<EmissionEvent>
	{
		public ParticleEmissionScript OnParticleEmission { get; set; }
	}
}
