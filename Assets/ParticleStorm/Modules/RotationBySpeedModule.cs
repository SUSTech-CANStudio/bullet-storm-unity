using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticleStorm.Core;
using UnityEngine;

namespace ParticleStorm.Modules
{
	internal sealed class RotationBySpeedModule : IParticleModule
	{
		public bool enabled;
		public Vector2 range;
		public ParticleSystem.MinMaxCurve x;
		public float xMultiplier = 1;
		public ParticleSystem.MinMaxCurve y;
		public float yMultiplier = 1;
		public ParticleSystem.MinMaxCurve z;
		public float zMultiplier = 1;

		public void ApplicateOn(PSParticleSystem ps)
		{
			var module = ps.GetComponent<ParticleSystem>().rotationBySpeed;
			module.enabled = enabled;
			if (enabled)
			{
				module.range = range;
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
