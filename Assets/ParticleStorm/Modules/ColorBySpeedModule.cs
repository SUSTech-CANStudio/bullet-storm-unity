using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticleStorm.Core;
using UnityEngine;

namespace ParticleStorm.Modules
{
	[Serializable]
	internal sealed class ColorBySpeedModule : IParticleModule
	{
		public bool enabled;
		public ParticleSystem.MinMaxGradient color;
		public Vector2 range;

		public void ApplicateOn(PSParticleSystem ps)
		{
			var module = ps.GetComponent<ParticleSystem>().colorBySpeed;
			module.enabled = enabled;
			module.color = color;
			module.range = range;
		}

		public void ApplicateOn(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}
	}
}
