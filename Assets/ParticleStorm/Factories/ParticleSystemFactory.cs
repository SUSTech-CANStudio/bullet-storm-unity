using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Core;
using ParticleStorm.Util;

namespace ParticleStorm.Factories
{
	internal static class ParticleSystemFactory
	{
		/// <summary>
		/// Tag of particle storm game objects.
		/// </summary>
		public static string particleTag { get => Settings.particleTag; }

		/// <summary>
		/// Create particle system particle.
		/// </summary>
		/// <returns></returns>
		public static PSParticleSystem PSParticleSystem()
		{
			var go = new GameObject();
			var p = go.AddComponent<PSParticleSystem>();
			if (particleTag != null && particleTag != "")
				go.tag = particleTag;
			return p;
		}

	}
}
