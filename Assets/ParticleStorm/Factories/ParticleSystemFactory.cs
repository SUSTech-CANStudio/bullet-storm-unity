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
		public static string ParticleTag { get => Settings.ParticleTag; }

		/// <summary>
		/// Create particle system particle.
		/// </summary>
		/// <returns></returns>
		public static PSParticleSystem PSParticleSystem()
		{
			var go = new GameObject();
			var p = go.AddComponent<PSParticleSystem>();
			if (ParticleTag != null && ParticleTag != "")
				go.tag = ParticleTag;
			return p;
		}

	}
}
