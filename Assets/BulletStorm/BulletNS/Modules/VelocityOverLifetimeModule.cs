#pragma warning disable 0649

using System;
using UnityEngine;

namespace BulletStorm.BulletNS.Modules
{
	[Serializable]
	internal struct VelocityOverLifetimeModule : IParticleModule
	{
		[Tooltip("Enable velocity change according to lifetime.")]
		public bool enabled;
		[Tooltip("Speed curve over lifetime.")]
		public ParticleSystem.MinMaxCurve speed;
		[Tooltip("Multiplier of speed.")]
		public float speedMultiplier;

		public void ApplicateOn(BulletSystemController psc)
		{
			var module = psc.ParticleSystem.velocityOverLifetime;
			module.enabled = enabled;
			if (module.enabled)
			{
				module.speedModifier = speed;
				module.speedModifierMultiplier = speedMultiplier;
			}
		}
	}
}
