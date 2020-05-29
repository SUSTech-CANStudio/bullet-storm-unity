#pragma warning disable 0649

using ParticleStorm.Core;
using System;
using UnityEngine;

namespace ParticleStorm.Modules
{
	[Serializable]
	internal struct SizeOverLifetimeModule : IParticleModule
	{
		[Tooltip("Enable size change according to lifetime.")]
		public bool enabled;
		[Tooltip("Curve to control particle size based on lifetime.")]
		public ParticleSystem.MinMaxCurve size;
		[Tooltip("Multiplier for size.")]
		public float sizeMultiplier;

		public void ApplicateOn(PSParticleSystem ps)
		{
			var module = ps.GetComponent<ParticleSystem>().sizeOverLifetime;
			module.enabled = enabled;
			if (enabled)
			{
				module.size = size;
				module.sizeMultiplier = sizeMultiplier;
			}
		}

		public void ApplicateOn(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}
	}
}
