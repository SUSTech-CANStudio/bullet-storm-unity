#pragma warning disable 0649

using System;
using UnityEngine;

namespace BulletStorm.BulletNS.Modules
{
	[Serializable]
	internal struct ColorBySpeedModule : IParticleModule
	{
		[Tooltip("Enable particle color change by its speed.")]
		public bool enabled;
		[Tooltip("The gradient that controls the particle colors.")]
		public ParticleSystem.MinMaxGradient color;
		[Tooltip("Apply the color gradient between these minimum and maximum speeds.")]
		public Vector2 range;

		public void ApplicateOn(BulletSystemController psc)
		{
			var module = psc.ParticleSystem.colorBySpeed;
			module.enabled = enabled;
			module.color = color;
			module.range = range;
		}
	}
}
