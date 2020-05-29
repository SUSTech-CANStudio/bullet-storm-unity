#pragma warning disable 0649

using ParticleStorm.Core;
using System;
using UnityEngine;

namespace ParticleStorm.Modules
{
	[Serializable]
	internal struct ColorOverLifetimeModule : IParticleModule
	{
		[Tooltip("Enable color change according to lifetime.")]
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
