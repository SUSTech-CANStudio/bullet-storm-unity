using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticleStorm.Core;
using UnityEngine;

namespace ParticleStorm.Modules
{
	internal sealed class RotationOverLifeTimeModule : IParticleModule
	{
		public bool enabled;
		public ParticleSystem.MinMaxCurve x;
		public float xMultiplier = 1;
		public ParticleSystem.MinMaxCurve y;
		public float yMultiplier = 1;
		public ParticleSystem.MinMaxCurve z;
		public float zMultiplier = 1;

		public void ApplicateOn(PSParticleSystem ps)
		{
			var module = ps.GetComponent<ParticleSystem>().rotationOverLifetime;
			module.enabled = enabled;
			if (enabled)
			{
				module.x = x;
				module.y = y;
				module.z = z;
				module.xMultiplier = xMultiplier;
				module.yMultiplier = yMultiplier;
				module.zMultiplier = zMultiplier;
			}
		}

		public void ApplicateOn(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}
	}
}
