#pragma warning disable 0649

using ParticleStorm.Core;
using System;
using UnityEngine;

namespace ParticleStorm.Modules
{
	[Serializable]
	internal struct RotationBySpeedModule : IParticleModule
	{
		[Tooltip("Enable rotation change according to speed.")]
		public bool enabled;
		[Tooltip("Set the minimum and maximum speeds that this module applies the rotation curve between.")]
		public Vector2 range;
		[Tooltip("Rotation by speed curve for the x-axis.")]
		public ParticleSystem.MinMaxCurve x;
		[Tooltip("Speed multiplier along the x-axis.")]
		public float xMultiplier;
		[Tooltip("Rotation by speed curve for the y-axis.")]
		public ParticleSystem.MinMaxCurve y;
		[Tooltip("Speed multiplier along the y-axis.")]
		public float yMultiplier;
		[Tooltip("Rotation by speed curve for the z-axis.")]
		public ParticleSystem.MinMaxCurve z;
		[Tooltip("Speed multiplier along the z-axis.")]
		public float zMultiplier;

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
