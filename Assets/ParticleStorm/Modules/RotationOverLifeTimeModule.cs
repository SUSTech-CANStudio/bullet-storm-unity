#pragma warning disable 0649

using ParticleStorm.Core;
using System;
using UnityEngine;

namespace ParticleStorm.Modules
{
	[Serializable]
	internal struct RotationOverLifetimeModule : IParticleModule
	{
		[Tooltip("Enable rotation change according to lifetime.")]
		public bool enabled;
		[Tooltip("Rotation by lifetime curve for the x-axis.")]
		public ParticleSystem.MinMaxCurve x;
		[Tooltip("Rotation multiplier along the x-axis.")]
		public float xMultiplier;
		[Tooltip("Rotation by lifetime curve for the y-axis.")]
		public ParticleSystem.MinMaxCurve y;
		[Tooltip("Rotation multiplier along the y-axis.")]
		public float yMultiplier;
		[Tooltip("Rotation by lifetime curve for the z-axis.")]
		public ParticleSystem.MinMaxCurve z;
		[Tooltip("Rotation multiplier along the z-axis.")]
		public float zMultiplier;

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
