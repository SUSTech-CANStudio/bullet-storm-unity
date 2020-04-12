using ParticleStorm.Core;
using System;
using UnityEngine;

namespace ParticleStorm.Modules
{
	[Serializable]
	internal sealed class ColorOverLifeTimeModule : IParticleModule
	{
		public bool enabled;
		public ParticleSystem.MinMaxGradient color;

		public void ApplicateOn(PSParticleSystem ps)
		{
			var module = ps.GetComponent<ParticleSystem>().colorOverLifetime;
			module.enabled = enabled;
			module.color = color;
		}

		public void ApplicateOn(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}
	}
}
