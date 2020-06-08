#pragma warning disable 0649

using System;
using UnityEngine;

namespace ParticleStorm.ParticleNS.Modules
{
	[Serializable]
	internal struct SizeBySpeedModule : IParticleModule
	{
		[Tooltip("Enable size change according to speed.")]
		public bool enabled;
		[Tooltip("Curve to control particle size based on speed.")]
		public ParticleSystem.MinMaxCurve size;
		[Tooltip("Multiplier for size.")]
		public float sizeMultiplier;
		[Tooltip("Set the minimum and maximum speed that this modules applies the size curve between.")]
		public Vector2 range;

		public void ApplicateOn(ParticleSystemController psc)
		{
			var module = psc.ParticleSystem.sizeBySpeed;
			module.enabled = enabled;
			if (enabled)
			{
				module.size = size;
				module.range = range;
				module.sizeMultiplier = sizeMultiplier;
			}
		}
	}
}
